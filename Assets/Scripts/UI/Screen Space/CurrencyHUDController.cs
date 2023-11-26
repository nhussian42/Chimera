using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyHUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bonesCount;
    [SerializeField] TextMeshProUGUI essenceCount;

    private void Start()
    {
        UpdateBones();
        UpdateEssence();
    }

    private void OnEnable()
    {
        CurrencyManager.OnBonesUpdate += UpdateBones;
        CurrencyManager.OnEssenceUpdate += UpdateEssence;
    }

    private void OnDisable()
    {
        CurrencyManager.OnBonesUpdate -= UpdateBones;
        CurrencyManager.OnEssenceUpdate -= UpdateEssence;
    }

    private void UpdateBones()
    {
        bonesCount.text = "" + CurrencyManager.Instance.currentBones;
    }

    private void UpdateEssence()
    {
        essenceCount.text = "" + CurrencyManager.Instance.currentEssence;
    }


}
