using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ShopCamera : MonoBehaviour
{

    private CinemachineVirtualCamera shopCam;
    void Awake()
    {
        shopCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        // InsertAction.Action += ShopView;
        // InsertAction.Action += DisableShopView;

    }
    private void OnDisable()
    {
        // InsertAction.Action -= ShopView;
        // InsertAction.Action -= DisableShopView;
    }

    public void ShopView()
    {
        //Coroutine to add delay to the cut + Fade to black?
        shopCam.Priority = 11;
        //Fade back in
    }

    public void DisableShopView()
    {
        //Coroutine to add delay to the cut + Fade to black
        shopCam.Priority = 0;
        //Fade back in
    }
}
