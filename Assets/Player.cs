using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Gun data")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Movement data")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private float verticalInput;
    private float horizontalInput;

    [Header("Tower data")]
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float towerRotationSpeed;

    [Header("Aim data")]
    [SerializeField] private Transform aimTransform;
    [SerializeField] private LayerMask whatIsAimMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
        CheckInputs();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyBodyRotation();
        ApplyTowerRotation();
    }

    private void CheckInputs()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }


        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Horizontal (Yatay) - A ve D tuşları
        float left = keyboard.aKey.isPressed ? 1f : 0f;
        float right = keyboard.dKey.isPressed ? 1f : 0f;
        horizontalInput = right - left;

        // Vertical (Dikey) - W ve S tuşları
        float up = keyboard.wKey.isPressed ? 1f : 0f;
        float down = keyboard.sKey.isPressed ? 1f : 0f;
        verticalInput = up - down;

        if (verticalInput < 0)
        {
            horizontalInput = -horizontalInput;
        }
    }

    private void UpdateAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAimMask))
        {
            float fixedY = aimTransform.position.y; // Y eksenini sabit tut
            aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }

    }

    private void ApplyMovement()
    {
        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void ApplyBodyRotation()
    {
        transform.Rotate(0, horizontalInput * rotationSpeed, 0);
    }

    private void ApplyTowerRotation()
    {
        Vector3 direction = aimTransform.position - towerTransform.position;
        direction.y = 0; // Y eksenini sabit tut

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        towerTransform.rotation = Quaternion.RotateTowards(towerTransform.rotation, targetRotation, towerRotationSpeed);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = gunPoint.forward * bulletSpeed;

        Destroy(bullet, 5f);
    }
}
