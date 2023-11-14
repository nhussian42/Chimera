using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Test/TestCoroutineSO", order = 1)]
public class TestCoroutineSO : Trinket
{
    [SerializeField] TrinketTimer timerPrefab;
    [SerializeField] private float time;
    private TrinketTimer timer;
    bool activated;

    public override void Enable()
    {
        activated = false;
        base.Enable();
    }

    public override void Activate()
    {
        Debug.Log("activated");
        if(activated == false)
        {
            if (timer == null) { timer = Instantiate(timerPrefab.gameObject).GetComponent<TrinketTimer>(); }
            timer.Play(time);
            activated = true;
        }
    }

    public override void ResetTrinket()
    {
        activated = false;
    }
}
