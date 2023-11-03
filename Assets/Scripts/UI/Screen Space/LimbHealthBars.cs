using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LimbHealthBars : MonoBehaviour
{

    public List<Slider> sliderList;
    public Gradient gradient;
    public List<Image> fillList;

    public void SetHealth(float health)
    {
        for (int i = 0; i < sliderList.Count; i++)
        {
            sliderList[i].value += (health / 2);

            fillList[i].color = gradient.Evaluate(sliderList[i].normalizedValue);
        }
    }

    public void SetMaxHealth(float health)
    {
        for (int i = 0; i < sliderList.Count; i++)
        {
            sliderList[i].maxValue = health;
            sliderList[i].value = health;

            fillList[i].color = gradient.Evaluate(1f);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            SetHealth(-5);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            SetHealth(5);
        }
    }
}
