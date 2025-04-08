using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement: MonoBehaviour
{

    private PlayerInput input = null;
    [SerializeField] GameObject mesh;
    [SerializeField] Rigidbody2D rb;
    public Rigidbody2D Rb {  get { return rb; } }

    private float horizontal = 0;
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    private bool isFacingRight = true;
    [SerializeField] private float acceleration=2;
    [SerializeField] private float decceleration=2;
    [SerializeField] private float velPower=2;

    [Header("Friction")]
    private float lastGroundTime;

    [Header("Jump")]
    private bool isJumping = false;
    [SerializeField] private float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask boxLayer;
    private bool doubleJump = false;
    [SerializeField] private GameObject jumpVueGO;
  

    [Header("WallJump")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    private bool isWallJumping;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(7, 15);

    [Header("JumpBuffering")]
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpBufferCounter;

    [Header("CoyoteTime")]
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float coyoteTimeCounter;

    [Header("Dash")]
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

  
    [Header("Gravity")]
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private float fallGravityMultiplier = 2f;

    [Header("Friction")]
    [SerializeField] private float frictionAmount = 0.3f;

    [Header("Particle")]
    public ParticleSystem particle;

    [Header("Checkpoint")]
    [SerializeField] private GameObject checkpoint;
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

        lastGroundTime -= Time.deltaTime;
        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        Move();
        Friction();
        Gravity();
        Jump();
        Dash();
        
    }
    /// <summary>
    /// Manage the gravity to fall faster more the falling is long
    /// </summary>
    private void Gravity()
    {
        //To have a grvity who accelerate when falling
        if (rb.velocity.y < 0)
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        else
            rb.gravityScale = gravityScale;
    }
   
    //Action
    /// <summary>
    /// Move the player with force
    /// </summary>
    public void Move()
    {
        horizontal = UserInput.instance.MoveInput.x;
        // rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        //Calculate the directin we want to move in and our desired velocity
        float targetSpeed = UserInput.instance.MoveInput.x * speed;
        //calculate difference between actual velocity and target velocity
        float speedDiff = targetSpeed - rb.velocity.x;
        //applies the accelearation for speed difference
        float accelRate = (Mathf.Abs(targetSpeed)>0.01f)?acceleration: decceleration;
        //calculate the acceleration to our movement
        float movement = Mathf.Pow(Mathf.Abs(speedDiff)* accelRate,velPower)*Mathf.Sign(speedDiff);
        rb.AddForce(Vector2.right * movement);

        if (!isFacingRight && horizontal > 0f)
            Flip();
        else if (isFacingRight && horizontal < 0f)
            Flip();

    }
    /// <summary>
    /// Manage the jump action
    /// </summary>
    public void Jump()
    {
        if (UserInput.instance.JumpInput)
        {
            if (!IsGrounded())
                WallJump();
            if ((IsGrounded() || doubleJump) && !isWallJumping && coyoteTimeCounter > 0f)
            { 
                StartCoroutine(DestroyVue());
                lastGroundTime = 0;
                coyoteTimeCounter = 0f;
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = !doubleJump;
                particle.Play();
            }
        }
    }
    /// <summary>
    /// Coroutine for debug to see where 
    /// </summary>
    private IEnumerator DestroyVue()
    {
        GameObject go = Instantiate(jumpVueGO, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(go);
    }
    /// <summary>
    /// Due to Physics Material's friction set to 0(to not block against a wall)
    /// </summary>
    private void Friction()
    {
        if (lastGroundTime > 0 && Mathf.Abs(UserInput.instance.MoveInput.x)<0.01)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount -= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    /// <summary>
    /// Start Dash coroutine when pressed
    /// </summary>
    public void Dash()
    {
        if (UserInput.instance.DashInput && canDash)
        {
            StartCoroutine(DashCoroutine());
        }


    }
    //Function
    /// <summary>
    /// Verify if the player touch the ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, boxLayer);
    }
    /// <summary>
    /// Verify if the player is against a wall
    /// </summary>
    /// <returns></returns>
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    /// <summary>
    /// Change the direction of the player 
    /// </summary>
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        if(IsGrounded())
            particle.Play();
    }
    /// <summary>
    /// Execute a wall against a wall
    /// </summary>
    private void WallJump()
    {
        if (IsWalled() && !isWallJumping)
        {
            //Debug.Log("WallJump");
            isWallJumping = true;
            rb.velocity = new Vector2(-transform.localScale.x * wallJumpingPower.x, wallJumpingPower.y);
            //rb.AddForce(new Vector2(-transform.localScale.x * wallJumpingPower.x, wallJumpingPower.y), ForceMode2D.Impulse);
            Flip();
            particle.Play();
            Invoke(nameof(StopWallJump),wallJumpingDuration);
        }
    }
    /// <summary>
    /// Stop the wall against the wall
    /// </summary>
    private void StopWallJump()
    {
        isWallJumping = false;
    }

    //Coroutine 
    /// <summary>
    /// Execute the action of dash
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Add speed to the player
    /// </summary>
    /// <param name="speedAdd"></param>
    public void AddSpeed(float speedAdd)
    {
        speed += speedAdd;
    }
   

    public void SetCheckpoint(GameObject _checkpoint)
    {
        checkpoint = _checkpoint;
    }
    public void TpToLastCheckpoint( )
    {
        transform.position = checkpoint.transform.position;
        GetComponent<PlayerHealth>().Revive();
        UnFreezePlayer();
    }


    public void FreezePlayer()
    {
        //block movements
        PlayerMovement.instance.enabled = false;
        PlayerMovement.instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //block interactions
    }
    public void UnFreezePlayer()
    {
        //block movements
        PlayerMovement.instance.enabled = true;
        PlayerMovement.instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //block interactions
    }
}
