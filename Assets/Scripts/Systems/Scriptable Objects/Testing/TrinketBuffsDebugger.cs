using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrinketBuffsDebugger : MonoBehaviour
{
    [SerializeField] ModifiedStatsSO modifiedStats;
    [SerializeField] CurrentBaseStatsSO currentBaseStats;

    [SerializeField] TextMeshProUGUI lArmClassText;
    [SerializeField] TextMeshProUGUI rArmClassText;
    [SerializeField] TextMeshProUGUI legsClassText;

    [SerializeField] TextMeshProUGUI lArmAtkDmgText;
    [SerializeField] TextMeshProUGUI lArmAtkSpdText;
    [SerializeField] TextMeshProUGUI lArmMaxHPText;
    [SerializeField] TextMeshProUGUI lArmHPText;

    [SerializeField] TextMeshProUGUI rArmAtkDmgText;
    [SerializeField] TextMeshProUGUI rArmAtkSpdText;
    [SerializeField] TextMeshProUGUI rArmMaxHPText;
    [SerializeField] TextMeshProUGUI rArmHPText;

    [SerializeField] TextMeshProUGUI legsMvSpdText;
    [SerializeField] TextMeshProUGUI legsCldnText;
    [SerializeField] TextMeshProUGUI legsMaxHPText;
    [SerializeField] TextMeshProUGUI legsHPText;

    bool isActive = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (isActive == true) { gameObject.SetActive(false); }
            else { gameObject.SetActive(true); }
        }

        lArmClassText.text = "L ARM CLASS: " + currentBaseStats.leftArmClass.value;
        rArmClassText.text = "R ARM CLASS: " + currentBaseStats.rightArmClass.value;
        legsClassText.text = "LEGS CLASS: " + currentBaseStats.legsClass.value;

        lArmAtkDmgText.text = "L ARM ATK DMG: +" + (modifiedStats.leftArmAttackDamage.value - currentBaseStats.leftArmAttackDamage.value);
        lArmAtkSpdText.text = "L ARM ATK SPD: +" + (modifiedStats.leftArmAttackSpeed.value - currentBaseStats.leftArmAttackSpeed.value);
        lArmMaxHPText.text = "L ARM ATK SPD: +" + (modifiedStats.leftArmMaxHealth.value - currentBaseStats.leftArmMaxHealth.value);
        lArmHPText.text = "L ARM ATK SPD: +" + (modifiedStats.leftArmHealth.value - currentBaseStats.leftArmHealth.value);

        rArmAtkDmgText.text = "R ARM ATK DMG: +" + (modifiedStats.rightArmAttackDamage.value - currentBaseStats.rightArmAttackDamage.value);
        rArmAtkSpdText.text = "R ARM ATK SPD: +" + (modifiedStats.rightArmAttackSpeed.value - currentBaseStats.rightArmAttackSpeed.value);
        rArmMaxHPText.text = "R ARM ATK SPD: +" + (modifiedStats.rightArmMaxHealth.value - currentBaseStats.rightArmMaxHealth.value);
        rArmHPText.text = "R ARM ATK SPD: +" + (modifiedStats.rightArmHealth.value - currentBaseStats.rightArmHealth.value);

        legsMvSpdText.text = "LEGS MV SPD: +" + (modifiedStats.legsMovementSpeed.value - currentBaseStats.legsMovementSpeed.value);
        legsCldnText.text = "LEGS CLDN: +" + (modifiedStats.legsCooldown.value - currentBaseStats.legsCooldown.value);
        legsMaxHPText.text = "LEGS MAX HP: +" + (modifiedStats.legsMaxHealth.value - currentBaseStats.legsMaxHealth.value);
        legsHPText.text = "LEGS HP: +" + (modifiedStats.legsHealth.value - currentBaseStats.legsHealth.value);

    }


}
