using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Slider healthbar;
    private void Start()
    {
        healthbar = GetComponent<Slider>();
    }
    private void Update()
    {

    }
    private void LateUpdate() 
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthbar.value = currentValue / maxValue;
        //Debug.Log("healthbar value: " + healthbar.value);
    }
}
