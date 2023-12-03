using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossNameText;
    [SerializeField] Slider bossHealthSliderR;
    [SerializeField] Slider bossHealthSliderL;
    [SerializeField] Slider damageHighlightR;
    [SerializeField] Slider damageHighlightL;

    [SerializeField] private float healthBarFillSpeed;
    [SerializeField] private float highlightSpeed;
    private float sinTime;

    private bool entered = false;
    float a = 100;
    float b = 100;
    bool damaged;

  private void Start()
    {
        entered = true;
        healthBarFillSpeed = healthBarFillSpeed / 1000;
        highlightSpeed = highlightSpeed / 100;
    }    
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) //Replace with boss taking damage
        {
            a = a - 20;
            UpdateHealthBar(a, b);
            
            //Invoke("ShowDamageHighlight", 1);
        }

        if (entered) //Meant for when to trigger the healthbar to go up (before/after cutscene?)
        {
            InitiateHealthBar();
        }

        if (damageHighlightL.value != bossHealthSliderL.value)
        {
            sinTime += Time.deltaTime * highlightSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = evaluate(sinTime);

            damageHighlightL.value = Mathf.Lerp(damageHighlightL.value, bossHealthSliderL.value, t);
            damageHighlightR.value = Mathf.Lerp(damageHighlightR.value, bossHealthSliderR.value, t);
            //Creates the highlight effect after boss takes damage
        }
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        bossHealthSliderL.value = currentHealth / maxHealth;
        bossHealthSliderR.value = currentHealth / maxHealth;
    }
    private void InitiateHealthBar() //Sets the slider values to max
    {
        bossHealthSliderR.value = bossHealthSliderR.value + healthBarFillSpeed;
        bossHealthSliderL.value = bossHealthSliderL.value + healthBarFillSpeed;
        if(bossHealthSliderR.value == 1)
        {
            entered = false;
            damageHighlightL.value = 1;
            damageHighlightR.value = 1; 
        }
    }

    private float evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
