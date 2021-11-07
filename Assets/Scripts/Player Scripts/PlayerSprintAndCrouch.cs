using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playermovement;
    public float sprint_Speed = 8f;
    public float move_Speed = 4f;
    public float crouch_Speed = 1.8f;
    private Transform lookRoot;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;
    private bool is_Crouching;

    private CharacterController characterController;
    private PlayerFootSteps player_Footsteps;
    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;
    
    //if we are sprinting we hear footstep more often if we crouch so more less.
    private float walk_Step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.25f; //accumulated//0.02f;
    private float crouch_Step_Distance = 0.5f;

    private PlayerStats playerStats;
    
    private float sprint_Value = 100f;
    [SerializeField]
    private float sprint_Treshold = 10f;

   

   
   void Awake()
    {
        playermovement = GetComponent<PlayerMovement>();
        lookRoot = transform.GetChild(0);
        player_Footsteps = GetComponentInChildren<PlayerFootSteps>();
        characterController=GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
    }
    void Start()
    {
        player_Footsteps.step_Distance = walk_Step_Distance;
        player_Footsteps.volume_Min = walk_Volume_Min;
        player_Footsteps.volume_Max = walk_Volume_Max;
        
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
        
    }
    void Sprint() {
        //if we are moving and sprint >0 
        if (sprint_Value > 0f) { 
        if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
        {
            playermovement.speed = sprint_Speed;
            player_Footsteps.step_Distance = sprint_Step_Distance;
            player_Footsteps.volume_Min = sprint_Volume;
            player_Footsteps.volume_Max = sprint_Volume;

        }

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            playermovement.speed = move_Speed;
            player_Footsteps.step_Distance = walk_Step_Distance;
            player_Footsteps.volume_Min = walk_Volume_Min;
            player_Footsteps.volume_Max = walk_Volume_Max;
        }
       
            if (Input.GetKey(KeyCode.LeftShift))
            {
            if (characterController.velocity.sqrMagnitude >0)
            {
                sprint_Value -= sprint_Treshold * Time.deltaTime;
                if (sprint_Value <= 0f)
                {
                    sprint_Value = 0f;
                    playermovement.speed = move_Speed;

                }

                playerStats.DisplayStaminaStats(sprint_Value);
            }

            }
            else
            {
                
                if (sprint_Value >= 100f)
                {
                    sprint_Value = 100f;
                }
                sprint_Value += (sprint_Treshold / 2f) * Time.deltaTime;

                playerStats.DisplayStaminaStats(sprint_Value);

            }
        

       


    }//sprint
    void Crouch() {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (is_Crouching)
            {//if we are crouching. -stand up
                lookRoot.localPosition = Vector3.Lerp(lookRoot.localPosition, new Vector3(0f, stand_Height, 0f), 1000f); 
                playermovement.speed = move_Speed;
                player_Footsteps.step_Distance = walk_Step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;
                is_Crouching = false;
            }
            else
            {// if we are not crouching -crouch
                lookRoot.localPosition = Vector3.Lerp(lookRoot.localPosition, new Vector3(0f, crouch_Height, 0f),1000f);
                playermovement.speed = crouch_Speed;
                player_Footsteps.step_Distance = crouch_Step_Distance;
                player_Footsteps.volume_Min = crouch_Volume;
                player_Footsteps.volume_Max = crouch_Volume;
                is_Crouching = true;
            }

        }//if input c
    
   
    }//crouch

}
