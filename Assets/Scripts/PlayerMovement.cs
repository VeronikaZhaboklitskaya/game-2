using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    public float bounceForce = 12f;

    [Header("Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.4f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool jumpPressed;

    private Rigidbody2D currentPlatformRb;
    private Vector2 lastPlatformPos;
    private Vector2 platformVelocity;
    private bool onPlatform;
    public float climbSpeed = 5f;
    private Collider2D currentTreeCollider;
    private bool isAttachedToTree;
    private Rigidbody2D currentTreeRb;
    private float originalGravityScale;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float xPosLastFrame;
    AudioManager audioManager;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 8f;
        originalGravityScale = rb.gravityScale;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isAttachedToTree && jumpPressed)
        {
            DetachFromTree();
        }

        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        FlipCharacterX();
    }

    void FixedUpdate()
    {
        if (isAttachedToTree)
        {
            ApplyTreeAttachment();
            return;
        }

        if (onPlatform && currentPlatformRb != null)
        {
            platformVelocity = (currentPlatformRb.position - lastPlatformPos) / Time.fixedDeltaTime;
            lastPlatformPos = currentPlatformRb.position;
        }
        else
        {
            platformVelocity = Vector2.zero;
        }

        Move();

        if (jumpPressed)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            jumpPressed = false;
        }
    }

    void Move()
    {

        float finalX = (moveInput * moveSpeed) + platformVelocity.x;
        float finalY = rb.linearVelocity.y;

        if (onPlatform && !jumpPressed && rb.linearVelocity.y <= 0.1f)
        {
            finalY = platformVelocity.y - 0.05f;
        }

        rb.linearVelocity = new Vector2(finalX, finalY);
    }

    private void FlipCharacterX()
    {
        moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0 && (transform.position.x > xPosLastFrame))
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput < 0 && (transform.position.x < xPosLastFrame))
        {
            spriteRenderer.flipX = false;

        }

        xPosLastFrame = transform.position.x;
    }

    void ApplyTreeAttachment()
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 move = new Vector2(
            horizontalInput * moveSpeed * 0.5f,
            verticalInput * climbSpeed
        ) * Time.fixedDeltaTime;

        Vector2 targetPos = rb.position + move;

        if (currentTreeCollider != null)
        {
            Bounds b = currentTreeCollider.bounds;

            float top = b.max.y - 0.5f;
            float bottom = b.min.y + 0.5f;

            targetPos.y = Mathf.Clamp(targetPos.y, bottom, top);
        }

        rb.MovePosition(targetPos);
    }


    void AttachToTree(Rigidbody2D treeRb)
    {
        isAttachedToTree = true;
        currentTreeRb = treeRb;
        currentTreeCollider = treeRb.GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    void DetachFromTree()
    {
        isAttachedToTree = false;
        currentTreeRb = null;
        rb.gravityScale = originalGravityScale;

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            jumpForce
        );
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingStone"))
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                currentPlatformRb = collision.rigidbody;
                if (!onPlatform)
                {
                    lastPlatformPos = currentPlatformRb.position;
                    onPlatform = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingStone"))
        {
            onPlatform = false;
            currentPlatformRb = null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Tree") && !isAttachedToTree)
        {
            if (rb.linearVelocity.y < 0.1f || collision.contacts[0].normal.y < 0.5f)
            {
                AttachToTree(collision.rigidbody);
                return;
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.hitfromabove);
                }
                    
                Destroy(collision.gameObject);

            }
            else
            {
                GameManager.instance.TakeDamage(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
        {
            GameManager.instance.FallDie();
        }
    }
}