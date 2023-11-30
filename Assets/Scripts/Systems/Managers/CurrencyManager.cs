using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    // Private
    public int currentBones { get; private set; }
    public int currentEssence { get; private set; }
    private bool firstLoad = false;

    // Exposed
    [SerializeField] private int defaultBones;
    [SerializeField] private int maxBones;
    [SerializeField] private float bonesMultiplier;
    [SerializeField] private int defaultEssence;
    [SerializeField] private int maxEssence;

    // Events
    public static Action OnBonesUpdate;
    public static Action OnEssenceUpdate;

    //protected override void Init()
    //{
    //    DontDestroyOnLoad(this);        
    //}

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        //subscribe to C# events here
        LimbSwapMenu.OnScrap += AddBones;
        MainMenu.StartPressed += InitializeBones;
    }

    private void OnDisable()
    {
        //unsubscribe to C# events here
        LimbSwapMenu.OnScrap -= AddBones;
        MainMenu.StartPressed -= InitializeBones;
    }

    private void InitializeBones()
    {
        currentBones = defaultBones;
        OnBonesUpdate?.Invoke();
        // Debug.Log("InitializeBones()");
    }

    private void ResetEssence()
    {
        currentEssence = defaultEssence;
        OnEssenceUpdate?.Invoke();
    }

    public void SetDefaultBonesValue(int newDefaultValue)
    {
        if (newDefaultValue >= 0)
            defaultBones = newDefaultValue;
    }

    public void SetMaxBonesValue(int newMaxValue)
    {
        if (newMaxValue > 0)
            maxBones = newMaxValue;
    }

    public void SetBonesMultiplier(float newMultiplierValue)
    {
        if (newMultiplierValue > 0)
            bonesMultiplier = newMultiplierValue;
    }

    private void AddBones(int amount)
    {
        if (bonesMultiplier > 1)
            amount *= (int)bonesMultiplier;
        currentBones = Mathf.Clamp(currentBones + amount, 0, maxBones);
        OnBonesUpdate?.Invoke();
    }

    private void AddEssence(int amount)
    {
        currentEssence = Mathf.Clamp(currentEssence + amount, 0, maxEssence);
        OnEssenceUpdate?.Invoke();
    }
}
