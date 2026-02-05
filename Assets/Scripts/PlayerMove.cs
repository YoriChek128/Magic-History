using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;

    private Rigidbody2D rb;
    private PlayerStats stats;
    private bool isSprinting;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        HandleInput();
        UpdateStamina();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        // Проверка на спринт
        if (Input.GetKeyDown(KeyCode.LeftShift) && stats.CurrentStamina > 0)
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || stats.CurrentStamina <= 0)
        {
            isSprinting = false;
        }

        // Обновляем скорость
        currentSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
    }

    private void MovePlayer()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;

        if (moveX != 0 && moveY != 0)
        {
            moveX *= 0.7071f;
            moveY *= 0.7071f;
        }

        Vector3 movement = new Vector3(moveX, moveY, 0) * currentSpeed * Time.fixedDeltaTime;
        transform.position += movement;
    }

    private void UpdateStamina()
    {
        if (isSprinting && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                           Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            stats.ChangeStamina(-stats.GetStaminaDrainAmount() * Time.deltaTime);
        }
        else if (!isSprinting && stats.CurrentStamina < stats.MaxStamina)
        {
            stats.ChangeStamina(stats.GetStaminaRegenAmount() * Time.deltaTime);
        }
    }
}