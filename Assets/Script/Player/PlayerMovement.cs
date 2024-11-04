using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public class PlayerMovement: MonoBehaviour
{

    private PlayerInput input = null;
    [SerializeField] GameObject mesh;
    [SerializeField] Rigidbody2D rb;

    private float horizontal = 0;

    private float speed = 8f;
    private bool isFacingRight = true;

    //Jump
    private float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool doubleJump = false;

    //Dash
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canDash = true;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    //Wall Jump
    public Transform wallCheck;
    public LayerMask wallLayer;
    private bool isWallJumping;
    private float wallJumpingDuration =0.4f;
    private float wallJumpingTime = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(7,13);

    //Particle
    public ParticleSystem particle;
    // [SerializeField] 

    public static PlayerMovement instance = null;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("PlayerManager is already set ");
            return;
        }
        instance = this;
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || isWallJumping)
        {
            return;
        }
        if (IsGrounded())
        {
            doubleJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing || isWallJumping)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal *speed,rb.velocity.y);
        if (!isFacingRight && horizontal > 0f)
            Flip();
        else if (isFacingRight && horizontal < 0f)
            Flip();
    }
    //Action
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        if(!IsGrounded())
            WallJump();
        if (context.performed && (IsGrounded() || doubleJump) && !isWallJumping)
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            doubleJump = !doubleJump;
            particle.Play();
        }


    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }


    }
    //Function
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        if(IsGrounded())
            particle.Play();
    }
    private void WallJump()
    {
        if (IsWalled() && !isWallJumping)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(-transform.localScale.x * wallJumpingPower.x, wallJumpingPower.y);
            Flip();
            particle.Play();
            Invoke(nameof(StopWallJump),wallJumpingDuration);
        }
    }
    private void StopWallJump()
    {
        isWallJumping = false;
    }
    private void Slide()
    {
        rb.AddForce(Vector2.up *-5f);
    }
    //Coroutine 

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        Debug.Log(rb.velocity);
        particle.Play();
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void AddSpeed(float speedAdd)
    {
        speed += speedAdd;
    }
    //Collision

}
