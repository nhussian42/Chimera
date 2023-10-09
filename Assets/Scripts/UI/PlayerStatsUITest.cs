using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUITest : MonoBehaviour
{
    [Header("Core Text")]
    [SerializeField] private TextMeshProUGUI coreHealth;
    
    [Header("Right Arm Text")]
    [SerializeField] private TextMeshProUGUI rightArmHealth;
    [SerializeField] private TextMeshProUGUI rightArmDamage;
    [SerializeField] private TextMeshProUGUI rightArmAttackSpeed;

    [Header("Left Arm Text")]
    [SerializeField] private TextMeshProUGUI leftArmHealth;
    [SerializeField] private TextMeshProUGUI leftArmDamage;
    [SerializeField] private TextMeshProUGUI leftArmAttackSpeed;


    private void OnEnable()
    {
        PlayerController.OnDamageReceived += UpdateHealthStats;
        PlayerController.OnArmSwapped += UpdateArmStats;
        PlayerController.OnArmSwapped += UpdateHealthStats;
    }

    private void OnDisable()
    {
        PlayerController.OnDamageReceived -= UpdateHealthStats;
        PlayerController.OnArmSwapped -= UpdateArmStats;
        PlayerController.OnArmSwapped -= UpdateHealthStats;
    }

    private void Start()
    {
        UpdateHealthStats();
        UpdateArmStats();
    }

    private void UpdateHealthStats()
    {
        coreHealth.text = $"Health: {PlayerController.Instance.CoreHealth}";
        leftArmHealth.text = $"Health: {PlayerController.Instance.currentLeftArm.Health}";
        rightArmHealth.text = $"Health: {PlayerController.Instance.currentRightArm.Health}";
    }

    private void UpdateArmStats()
    {
        leftArmDamage.text = $"Damage: {PlayerController.Instance.currentLeftArm.AttackDamage}";
        leftArmAttackSpeed.text = $"Attack Speed: {PlayerController.Instance.currentLeftArm.AttackSpeed}";
        rightArmDamage.text = $"Damage: {PlayerController.Instance.currentRightArm.AttackDamage}";
        rightArmAttackSpeed.text = $"Attack Speed: {PlayerController.Instance.currentRightArm.AttackSpeed}";
    }
}
