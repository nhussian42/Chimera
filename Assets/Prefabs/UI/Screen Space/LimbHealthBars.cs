using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LimbHealthBars : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetHealth(float health)
    {
        slider.value += health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
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
