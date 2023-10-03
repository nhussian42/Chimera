using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmDrop : MonoBehaviour
{
    [SerializeField] private Arm ArmToDrop;
    public Arm armReference { get; private set; }

    private void OnEnable()
    {
        armReference = ArmToDrop;
    }


}
