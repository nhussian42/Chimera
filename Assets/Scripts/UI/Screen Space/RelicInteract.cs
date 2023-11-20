using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject RelicMenu;
    private bool menuToggle;
    public bool pickedYes;

    public static RelicInteract instance;
    void Start()
    {
        menuToggle = false;
        RelicMenu = Instantiate(RelicMenu, new Vector3(0, 0, 0), Quaternion.identity);
        RelicMenu.SetActive(false);
        instance = this;
        pickedYes = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected");
        if (other.CompareTag("Player") && (menuToggle == false))
        {

                RelicMenu.SetActive(true);
                menuToggle = !menuToggle;
                //if (menuToggle) PlayerController.Instance.Pause();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R) && !menuToggle && !pickedYes)
        {
            Debug.Log("Player Pressed R");
            RelicMenu.SetActive(true);
            menuToggle = !menuToggle;
            if (menuToggle) PlayerController.Instance.Pause();

        }
        if (Input.GetKeyDown(KeyCode.R) && menuToggle)
        {
            RelicMenu.SetActive(false);
       
            if (menuToggle == true) UIManager.ResumePressed();
            menuToggle = !menuToggle;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        menuToggle = false;
    }

}
