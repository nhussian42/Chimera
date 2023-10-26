using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Test/TestCoroutineSO", order = 1)]
public class TestCoroutineSO : EventListener
{
    public TrinketTimer trinketTimer;
    [SerializeField] private float time;

    public override void Activate()
    {
        TrinketTimer timer = Instantiate(trinketTimer.gameObject).GetComponent<TrinketTimer>();
        timer.Play(time);
    }
}
