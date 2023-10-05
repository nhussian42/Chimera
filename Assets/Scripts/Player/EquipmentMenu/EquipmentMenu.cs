using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMenu : MonoBehaviour
{
    public GameObject equipmentMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            equipmentMenu.gameObject.SetActive(!equipmentMenu.gameObject.activeSelf);
        }
    }
}
