using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image health_Stats, stamina_Stats;

   
    public void DisplayHealthStats(float healthValue)
    {
        healthValue /= 100f;
        health_Stats.fillAmount = healthValue;


    }
    public void DisplayStaminaStats(float staminaValue)
    {
        staminaValue /= 100f;
        stamina_Stats.fillAmount = staminaValue;
    }


}
