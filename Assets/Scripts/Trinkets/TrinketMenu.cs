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

    public Canvas canvas;

    public GameObject TrinketOptionMenu;

    public void CloseMenu()
    {
        if (active == false)
        {
            canvas.enabled = true;
            active = true;
        }
        if (active == true)
        {
            canvas.enabled = false;
            active = false;
        }
    }







}
