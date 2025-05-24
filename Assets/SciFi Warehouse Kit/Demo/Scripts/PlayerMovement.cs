using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;

    public Camera mainCamera;

    // Ссылка на текущую комнату
    public Room currentRoom;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 motion = transform.right * x + transform.forward * z;
        controller.Move(motion * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) && isGrounded)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null && footStepSound != null)
                {
                    audio.PlayOneShot(footStepSound, 0.7f);
                }
                nextFootstep += footStepDelay;
            }
        }

        RotateTowardCursor();
    }

    void RotateTowardCursor()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, groundMask))
        {
            Vector3 targetPos = hitInfo.point;
            targetPos.y = transform.position.y;
            Vector3 direction = targetPos - transform.position;

            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 15f * Time.deltaTime);
            }
        }
    }
}
