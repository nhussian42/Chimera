using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTrinketManager : Singleton<NewTrinketManager>
{
    [SerializeField] private MasterTrinketList masterTrinketList;

    protected override void Init()
    {
        masterTrinketList.FullReset();
    }


}
