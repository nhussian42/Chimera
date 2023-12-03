using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoHead : Head
{
    [SerializeField] GameObject vfxPrefab;
    public GameObject VFXPrefab { get { return vfxPrefab; } }
}
