using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RelicInteract : MonoBehaviour
{
    
    [SerializeField] private GameObject RelicMenu;
    private bool menuToggle;
    public bool pickedYes;
    private bool isInside;
    private bool canClose;

    private PlayerController playerController;

    public static RelicInteract instance;
    void Start()
    {
        menuToggle = false;
        RelicMenu = Instantiate(RelicMenu, new Vector3(0, 0, 0), Quaternion.identity);
        RelicMenu.SetActive(false);
        instance = this;
        pickedYes = false;
        playerController = PlayerController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (menuToggle == false))
        {
            isInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
        menuToggle = false;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !menuToggle && !pickedYes && isInside)
        {
            RelicMenu.SetActive(true);
            menuToggle = !menuToggle;
            playerController.DisableAllDefaultControls();
            playerController.EnableAllUIControls();
            Invoke("DelayClose", 1);
        }

        if (Input.GetKeyDown(KeyCode.R) && menuToggle && canClose && isInside)
        {
            RelicMenu.SetActive(false);
            menuToggle = !menuToggle;
            playerController.EnableAllDefaultControls();
            playerController.DisableAllUIControls();
            canClose = false;
        }
        
    }

    private void DelayClose()
    {
        canClose = true;
    }


}
