using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class andar : MonoBehaviour
{
    [Header("Movimento")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float rotationSpeed = 10f;

    [Header("Pulo")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity; // para gravidade
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Checa se está no chão
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // mantém o player preso ao chão
        }

        // Entrada WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Corre se estiver segurando LeftShift
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            // Move o player
            controller.Move(direction * currentSpeed * Time.deltaTime);

            // Rotaciona suavemente para onde anda
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Fórmula da velocidade inicial do pulo
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplica gravidade
        velocity.y += gravity * Time.deltaTime;

        // Move player verticalmente
        controller.Move(velocity * Time.deltaTime);
    }
}
