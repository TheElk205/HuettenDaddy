using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class SkiingController : MonoBehaviour
    {
        //used to store the player input value, stores and x (left/right) and y (up/down) value
        private Vector2 playerInputValue;
        public GroundCheck groundCheck;
        private bool canJump = true;
        public PlayerState playerState = PlayerState.Idle;
        public float checkBottomAfterJumpLength = 1.0f;
        public float dragDown = 1.0f;
        public float privateRotation = 0.0f;
        public float movementSpeed = 1.0f;
        public float delayCheckJumpFinished = 1.0f;
        public SpriteRenderer sprite;
        public float rotationFactor = 10.0f;
        public float currentRotation = 0.0f;
        private Quaternion rotateTowards = Quaternion.identity;
        public Animator animator;
        
        private float jumpStarted = 0.0f;
        //Store the hit information from our raycast, to use to update player's position
        private RaycastHit2D Hit2D;
        
        void Start()
        {
            groundCheck = GetComponent<GroundCheck>();
            this.sprite = this.transform.GetComponentInChildren<SpriteRenderer>();
            animator = this.GetComponentInChildren<Animator>();
        }

        public void Update()
        {
            if (GameEventSystem.currentGameState != GameState.Playing) return;

            if (playerState == PlayerState.Idle) playerState = PlayerState.Skiing;
            
            if (playerState == PlayerState.Skiing)
            {
                MoveLogic();
                if (sprite)
                {
                    rotateTowards = Quaternion.Euler(new Vector3(0,0,groundCheck.myAngle));
                }
            }

            if (currentRotation != 0)
            {
                rotateTowards = Quaternion.Euler(new Vector3(0, 0,
                    sprite.transform.rotation.eulerAngles.z + rotationFactor * currentRotation * Time.deltaTime));
            }
            sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, rotateTowards, 0.05f);
        }

        public void FixedUpdate()
        {
            if (GameEventSystem.currentGameState != GameState.Playing) return;
            if (playerState == PlayerState.Jumping && Time.time > jumpStarted + delayCheckJumpFinished)
            {
                HitBottomCheck();
            }
        }

        //This method is derived from our player input component/input actions
        //We created an action called Move in our Input action asset.
        //The player input component on our player sends meesages via the action names.
        //Example: Move turns to OnMove, if we had an action called Run, it would send a message to OnRun, ect..
        private void OnMove(InputValue value)
        {
            //store player input value in form of vector2. left/right is x values, up/down is y values
            playerInputValue = value.Get<Vector2>();
        }
        
        private void OnJump()
        {
            if (groundCheck && playerState == PlayerState.Skiing)
            {
                animator.SetBool("Jumping", true);
                playerState = PlayerState.Jumping;
                groundCheck.enabled = false;
                Debug.Log("Jump");
                // Vector2 temp = new Vector2(0, 2);
                // transform.Translate(temp);
                StartCoroutine(JumpMovement());
            }
            else
            {
                Debug.Log("Can not jump");
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DoNotTouch"))
            {
                animator.SetTrigger("Hurt");
            }
        }

        IEnumerator JumpMovement()
        {
            Vector3 end = transform.position;
            end.x += 5;
            jumpStarted = Time.time;
            
            // Calculate distance to target
            float target_Distance = Vector3.Distance(transform.position, end);
            Debug.Log("Distance to target: " + target_Distance);
            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * privateRotation * Mathf.Deg2Rad) / dragDown);
            Debug.Log("Velocity: " + projectile_Velocity);
            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(privateRotation * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(privateRotation * Mathf.Deg2Rad);
     
            // Calculate flight time.
            float flightDuration = target_Distance / Vx;
            Debug.Log("Flight duration to target: " + flightDuration);

            yield return null;
            yield return null;
            
            float elapse_time = 0;
            while (true)
            {
                if (playerState != PlayerState.Jumping)
                {
                    break;
                }
                transform.parent.Translate (0, (Vy - (dragDown * elapse_time)) * Time.deltaTime, 0);
                transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
     
                elapse_time += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Finished jump");
            animator.SetBool("Jumping", false);
        }

        private void HitBottomCheck()
        {
            Hit2D = Physics2D.Raycast(groundCheck.rayCastOrigin.position, -Vector2.up, checkBottomAfterJumpLength, groundCheck.layerMask);
            Debug.DrawRay(groundCheck.rayCastOrigin.position, -Vector2.up * checkBottomAfterJumpLength, Color.cyan);
            //Performant check to see if raycast hit has any data, if so, run the code
            if (Hit2D)
            {
                Debug.Log("We hit the ground again, ending jump");
                playerState = PlayerState.Skiing;
                groundCheck.enabled = true;
                currentRotation = 0.0f;
                float angle = (sprite.transform.rotation.eulerAngles.z + Vector2.SignedAngle(Hit2D.normal, Vector2.up)) % 360;
                Debug.Log("We hit the bottom with an angle of: " + angle);
            }
        }
        private void OnRotate(InputValue value)
        {
            if (playerState != PlayerState.Jumping) return;
            
            Debug.Log("rotation");
            float rotation = value.Get<float>();
            currentRotation = rotation;
        }

        private void MoveLogic()
        {
            //Create temp vector2 which stores the x value of player input and 0 for y. 
            Vector2 temp = new Vector2(movementSpeed * Time.deltaTime, 0);
            //translate our player transform via the temp vector2 amount
            transform.Translate(temp);
        }
    }

    public enum PlayerState
    {
        Idle,
        Skiing,
        Jumping
    }
}