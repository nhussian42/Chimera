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


    public PlayerController PlayerController;

    public GameObject self;

    public void CloseMenu()
    {
      self.SetActive(false);
    }

    public void IncreaseDamage()
    {
        PlayerController.Instance.currentLeftArm.UpdateAttackDamage(500);
        PlayerController.Instance.currentRightArm.UpdateAttackDamage(5);
        CloseMenu();
    }

    public void IncreaseATKSpeed()
    {
        PlayerController.Instance.currentLeftArm.UpdateAttackSpeed(0.05f);
        PlayerController.Instance.currentRightArm.UpdateAttackSpeed(0.05f);
        CloseMenu();
    }

    public void IncreaseHealth()
    {

        PlayerController.Instance.currentLeftArm.UpdateMaxHealth(5);
        PlayerController.Instance.currentRightArm.UpdateMaxHealth(5);
        PlayerController.Instance.currentRightArm.DebugLog();
        CloseMenu();
    }






}
