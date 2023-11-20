using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMController : MonoBehaviour
{
    [SerializeField] private EMScript equipUI;

    public int invSize = 10;

    private void Start()
    {
        //equipUI.InitializeTrinketInv(invSize);
    }
}
