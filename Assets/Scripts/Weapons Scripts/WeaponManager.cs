using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{ //current_selected_index ,selectedindex;
    // Start is called before the first frame update
    [SerializeField]
    private WeaponHandler[] weapons;

    private int current_selected_index;
    void Start()
    {
        //as soon as the game starts we wanna see the axe.
        current_selected_index = 0;
        weapons[current_selected_index].gameObject.SetActive(true);
    
                
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnSelectedWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnSelectedWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnOnSelectedWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TurnOnSelectedWeapon(5);
        }

    }//update

    void TurnOnSelectedWeapon(int weaponIndex)
    {
        //if we dont wanna draw the weapon we selected 
        if (current_selected_index == weaponIndex)
            return;
        //turn off the current weapon
        weapons[current_selected_index].gameObject.SetActive(false);
        //turn on the selected weapon
        weapons[weaponIndex].gameObject.SetActive(true);
        //store the current selected weapon index
        current_selected_index = weaponIndex;
        

    }
    //we need to return to info which weapon we  selected.
    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[current_selected_index];
       

    }
}//class
