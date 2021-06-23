using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cam;
    public Animator animator;
    public Animator animator2;
    private bool controls;
    private bool startGame;



    //Lives & Health
    private int lives = 10;
    public Text livesTest;
    private bool isDead = false;
    public LayerMask deathMask;
    public GameObject checkPointSpawner;
    private bool canDie = true;

    //model change-Animation
    //public Transform baseModel;
    //public Transform jumpModel;


    //"Sprint also meaning Dash controls
    public static float normalSpeed = 8f;
    private float speed = normalSpeed;
    private float sprintMultiplier = 2.45f;
    private bool sprintBool = false;
    private bool canSprint = false;
    private bool setSprintTimer = false;
    private bool isDashing = false;
    public static float SprintTimer = 1f;


    //Dash can't rejump
    private bool cantJump = false;


    //smooth turning
    private float turnSmoothTime = 0.08f;
    float turnSmoothVelocity;

    private Vector3 movementVector;

    //Jump
    public int jumpVelocity = 10;

    //JumpGravity Physics
    private float gravity = -11f;
    Vector3 velocity;
    private float fallMultiplier = 1.6f;
    private float lowJumpMultiplier = 4.2f;

    //ground check
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    bool isGrounded;

    //wall check
    public Transform wallCheck;
    public float wallDistance = 1.5f;
    public LayerMask wallMask;
    bool onWall;
    bool wallStuckMode;
    bool canWallJump = true;

    //Speed on how fast the jump physics will fall
    public float gravityController = 10;


    // Start is called before the first frame update
    void Start()
    {
        lives = 10;
        Cursor.lockState = CursorLockMode.Locked;
        livesTest.text = lives.ToString();
        controls = false;
        startGame = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (startGame == true && controls == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                controls = true;
            }
        }

        if (controls == true)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            onWall = Physics.CheckSphere(wallCheck.position, wallDistance, wallMask);
            isDead = Physics.CheckSphere(wallCheck.position, wallDistance, deathMask);

            if (onWall == true && isGrounded == false && canWallJump == true)
            {
                if (wallStuckMode == false)
                {
                    Debug.Log("haha funny wall time");
                    WallStuck();
                }
                else if (onWall == false)
                {
                    NotifyEndWall();
                }
            }

            if (canDie == true)
            {

                if (isDead == true)
                {
                    controls = false;
                    canDie = false;
                    isDead = false;
                    transform.position = checkPointSpawner.transform.position;
                    lives = lives - 1;
                    if (lives > 0)
                    {
                        livesTest.text = lives.ToString();
                        DeathScript();
                    }

                    else
                    {
                        livesTest.text = lives.ToString();
                        DeathScript();
                        Debug.Log("Game Over");
                        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
                    }
                }
            }

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -1f;
            }

            //movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            //falling gravity
            if (sprintBool == false && wallStuckMode == false)
            {
                velocity.y += gravity * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);
            }



            //simple dash command check
            Dash();
            //check is dashing while entering wallstuck mode
            //if (setSprintTimer == true && wallStuckMode == true)
            if (wallStuckMode == true)
            {
                sprintBool = false;
                canWallJump = false;
                canSprint = true;
                speed = 0;
                //velocity.y = Mathf.Sqrt(jumpVelocity * -2f * gravity);
                velocity.y = -1f;
                setSprintTimer = false;
                Debug.Log("AHH");
                NotifyEndDash();
                WallStuck();
            }

            //Moving - angle changes
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir * speed * Time.deltaTime);
            }

            //Jump gravity controller
            if (velocity.y < 1.1 && wallStuckMode == false)
            {
                velocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (velocity.y > 1.1 && !Input.GetButton("Jump") && wallStuckMode == false)
            {
                velocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //Reset Velocity on Land
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpVelocity * -2f * gravity);
                canSprint = true;
                wallStuckMode = false;
                canWallJump = true;
            }

            //Reset Velocity on Land
            if (Input.GetButtonDown("Jump") && onWall && wallStuckMode == true)
            {
                canSprint = true;
                wallStuckMode = false;
                canWallJump = false;
                NotifyEndWall();
                Debug.Log("WallJump");
                velocity.y = Mathf.Sqrt(jumpVelocity * -2f * gravity);

            }

            //Air Smooth movement
            if (isGrounded == false)
            {
                turnSmoothTime = 0.2f;
            }
            else if (isGrounded == true)
            {
                turnSmoothTime = 0.08f;
            }


        }
    }

    void Dash()
    {
        //Sprint Controls
        if (Input.GetKeyDown(KeyCode.LeftShift) && canSprint == true && isGrounded == false && wallStuckMode == false)
        {
            sprintBool = true;
            isDashing = true;
            Debug.Log("DashStart");
            speed = speed * sprintMultiplier;
            canSprint = false;
            setSprintTimer = true;
            StartCoroutine(DashUpdate());
        }
    }

    IEnumerator DashUpdate()
    {
        if (setSprintTimer == true)
        {
            yield return new WaitForSeconds(0.3f);
            if (wallStuckMode == false)
            {
                sprintBool = false;
                speed = normalSpeed;
                //velocity.y = Mathf.Sqrt(jumpVelocity * -2f * gravity);
                velocity.y = -1f;
                setSprintTimer = false;
                NotifyEndDash();
            }

        }
    }


    void NotifyEndDash()
    {
        Debug.Log("DashEnd");
        isDashing = false;
    }

    //wall jump
    void WallStuck()
    {
        if (canWallJump == true)
        {
            canWallJump = false;
            velocity.y = 0f;
            wallStuckMode = true;
            animator.SetBool("SlugBall", true);
            animator2.SetBool("SlugBall2", true);
            StartCoroutine(WallStuckUpdate());
        }
    }

    IEnumerator WallStuckUpdate()
    {
        yield return new WaitForSeconds(2);
        NotifyEndWall();
    }

    void NotifyEndWall()
    {
        
        Debug.Log("Fall");
        wallStuckMode = false;
        if (isDashing == false)
        {
            speed = normalSpeed;
        }
        animator.SetBool("SlugBall", false);
        animator2.SetBool("SlugBall2", false);
    }


    //death void

    void DeathScript()
    {
        canWallJump = false;
        canSprint = true;
        speed = 0;
        sprintBool = false;
        //wallStuckMode = true;
        velocity.y = 0f;
        canSprint = false;
        //characterController.transform.position = checkPointSpawner.transform.position;
        StartCoroutine(DeathScriptWait());
    }

    IEnumerator DeathScriptWait() //A little delay so collision glitches don't happen and more lives get taken off then needed
    {
        yield return new WaitForSeconds(0.05f);
        transform.position = checkPointSpawner.transform.position;
        yield return new WaitForSeconds(0.05f);
        DeathEnd();
        
    }

    void DeathEnd() //Resetting to Normal Controls after death and respawning
    {
        Debug.Log("new life");
        speed = normalSpeed;
        canDie = true;
        isDead = false;
        controls = true;
    }



    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //        Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
    //}



    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Tomato"))
    //    {
    //        Destroy(other.gameObject);
    //    }
    //    else if (other.gameObject.CompareTag("Leaf"))
    //    {
    //        Destroy(other.gameObject);
    //    }
    //    else if (other.gameObject.CompareTag("Key"))
    //    {
    //        Destroy(other.gameObject);
    //    }

    //}


}
