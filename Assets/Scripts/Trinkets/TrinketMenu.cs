using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrinketMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public Button Option1;
    public Button Option2;
    public Button Option3;

    public bool active = false;

    public PlayerController playerController;

    public GameObject TrinketOptionMenu;

    public void CloseMenu()
    {
      //TrinketOptionMenu.SetActive(false);
    }

    public void IncreaseDamage()
    {
        // playerController.health += 5;
        TrinketOptionMenu.SetActive(false);
    }

    public void IncreaseATKSpeed()
    {
        // playerController.atkspeed += 5;
        TrinketOptionMenu.SetActive(false);
    }

    public void IncreaseHealth()
    {
        // playerController.damage += 5;
        TrinketOptionMenu.SetActive(false);
    }






}
