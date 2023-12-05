using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : Singleton<BossHealthBarUI>
{
    [SerializeField] TextMeshProUGUI bossNameText;
    [SerializeField] Slider bossHealthSliderR;
    [SerializeField] Slider bossHealthSliderL;
    [SerializeField] Slider damageHighlightR;
    [SerializeField] Slider damageHighlightL;

    [SerializeField] private float healthBarFillSpeed;
    [SerializeField] private float highlightSpeed;
    private float sinTime;
    [SerializeField] private GameObject self;
    public bool entered = false;
    private bool healthBarFull;
    float a = 100;
    float b = 100;
    bool damaged;

  private void Start()
    {
        healthBarFillSpeed = healthBarFillSpeed / 1000;
        highlightSpeed = highlightSpeed / 100;
        bossHealthSliderL.value = 0;
        bossHealthSliderR.value = 0;

        
    }    
    
    private void Update()
    {
        Grolfino.BossDead += OnBossDie;
        if(bossHealthSliderR.value < .9 && healthBarFull)
        {
            highlightSpeed = .0025f;
        }

        if (damageHighlightL.value != bossHealthSliderL.value && healthBarFull)
        {
            sinTime += Time.deltaTime * highlightSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = evaluate(sinTime);

            damageHighlightL.gameObject.SetActive(true);
            damageHighlightR.gameObject.SetActive(true);
            
            damageHighlightL.value = Mathf.Lerp(damageHighlightL.value, bossHealthSliderL.value, t);
            damageHighlightR.value = Mathf.Lerp(damageHighlightR.value, bossHealthSliderR.value, t);
            //Creates the highlight effect after boss takes damage
        }

        if(entered)
        {       
            InitiateHealthBar();
        }
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        bossHealthSliderL.value = currentHealth / maxHealth;
        bossHealthSliderR.value = currentHealth / maxHealth;
    }
    public void InitiateHealthBar() //Sets the slider values to max
    {       
        bossHealthSliderR.value = bossHealthSliderR.value + healthBarFillSpeed;
        bossHealthSliderL.value = bossHealthSliderL.value + healthBarFillSpeed;
        if(bossHealthSliderR.value == 1)
        {
            healthBarFull = true;
            damageHighlightL.value = 1;
            damageHighlightR.value = 1;    
            entered = false;
        }
    }

    private float evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }

    private void OnBossDie()
    {
        self.SetActive(false);
    }
}
