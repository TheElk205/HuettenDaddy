using System;
using System.Collections;
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
        public PlayerState playerState = PlayerState.Skiing;
        public float checkBottomAfterJumpLength = 1.0f;
        public float dragDown = 1.0f;
        public float privateRotation = 0.0f;
        public float movementSpeed = 1.0f;
        public float delayCheckJumpFinished = 1.0f;
        public SpriteRenderer sprite;
        
        private Quaternion rotateTowards = Quaternion.identity;
        
        private float jumpStarted = 0.0f;
        //Store the hit information from our raycast, to use to update player's position
        private RaycastHit2D Hit2D;
        
        void Start()
        {
            groundCheck = GetComponent<GroundCheck>();
            this.sprite = this.transform.GetComponentInChildren<SpriteRenderer>();
        }

        public void Update()
        {
            if (playerState == PlayerState.Jumping && Time.time > jumpStarted + delayCheckJumpFinished)
            {
                HitBottomCheck();
            }

            else if (playerState == PlayerState.Skiing)
            {
                MoveLogic();
                if (sprite)
                {
                    rotateTowards = Quaternion.Euler(new Vector3(0,0,groundCheck.myAngle));
                }
            }
            
            sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, rotateTowards, 0.05f);
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
            }
        }
        private void OnRotate(InputValue value)
        {
            Debug.Log("rotation");
            float rotation = value.Get<float>();
            Debug.Log(rotation);
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
        Skiing,
        Jumping
    }
}