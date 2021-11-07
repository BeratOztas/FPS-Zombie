using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character_controller;
    private Vector3 move_Direction;
    public float speed = 5f;
    public float jump_Force = 10f;
    private float gravity = 20f;
    private float vertical_Velocity;
    private float minX= 0.3f, maxX=255f;
    private float minZ=0.3f, maxZ = 255f;
    void Awake()
    {
        character_controller = GetComponent<CharacterController>();  
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();  
    }
    void MoveThePlayer()
    {

        move_Direction = 
         new Vector3(Input.GetAxis(Axis.HORIZONTAl), 0f, Input.GetAxis(Axis.VERTICAL));
        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;
        ApplyGravity();//we apply gravity and jump then we move our controller.
        character_controller.Move(move_Direction);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y,
            Mathf.Clamp(transform.position.z, minZ, maxZ));


    }//move player
    void ApplyGravity() {
        
            vertical_Velocity -= gravity * Time.deltaTime;
            //jump
            PlayerJump();
      
        //we added time*delta cause too much value;
        move_Direction.y = vertical_Velocity * Time.deltaTime; 
    }//applyGravity

    void PlayerJump() { 
    if(character_controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }
    }

}//class
