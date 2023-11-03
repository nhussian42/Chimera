using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private Arm savedLeftArm;
    private Arm savedRightArm;
    private Core savedCore;
    private Legs savedLegs;

    public bool firstLoad { get; private set; }

    // Public getters
    public Arm SavedLeftArm { get { return savedLeftArm; } }
    public Arm SavedRightArm { get { return savedRightArm; } }
    public Core SavedCore { get { return savedCore; } }
    public Legs SavedLegs { get { return savedLegs; } }

    protected override void Init()
    {
        firstLoad = true;
        DontDestroyOnLoad(this);
    }

    public void SaveLimbData(Arm leftArm, Arm rightArm, Core core, Legs legs)
    {
        // Add legs and head data here and in parameters later
        savedLeftArm = leftArm;
        savedRightArm = rightArm;
        savedCore = core;
        savedLegs = legs;
        firstLoad = false;
        //Debug.Log("Data saved");
        //Debug.Log(savedLeftArm.AttackDamage);
        //Debug.Log(savedLeftArm.AttackSpeed);
        //Debug.Log(savedLeftArm.Health);
    }
}
