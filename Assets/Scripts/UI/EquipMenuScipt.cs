using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipMenuScipt : MonoBehaviour
{
    [SerializeField] private GameObject EquipMenu;
    private bool EM = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown("e") && !EM)
        {
            EquipMenu.SetActive(true);
            EM = true;
        } else if (Input.GetKeyDown("e") && EM)
        {
            EquipMenu.SetActive(false);
            EM = false;
        }
    }
}
