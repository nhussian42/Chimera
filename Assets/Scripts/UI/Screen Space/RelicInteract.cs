using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject RelicMenu;
    private bool menuToggle;
    public bool pickedYes;
    private bool isInside;

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
            isInside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R) && menuToggle)
        {
            RelicMenu.SetActive(false);
       
            if (menuToggle == true) UIManager.ResumePressed();
            menuToggle = !menuToggle;
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
            Debug.Log("Player Pressed R");
            RelicMenu.SetActive(true);
            menuToggle = !menuToggle;

        }
    }
}
