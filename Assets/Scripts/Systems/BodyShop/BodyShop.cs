using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class BodyShop : MonoBehaviour
{
    
    public List<GameObject> ArmList;
    public List<GameObject> LegList;
    public List<GameObject> HeadList;
    public List<GameObject> ItemLocations;

    public GameObject ShopMenu;
    public bool IsMenuActive;

    int randomArm;
    int randomLeg;
    int randomHead;


    public GameObject HealItem;

    // Start is called before the first frame update
    void Start()
    {

        RandomizeOptions();

        Instantiate(ArmList[randomArm], ItemLocations[0].transform.position, Quaternion.identity);
        Instantiate(LegList[randomLeg], ItemLocations[1].transform.position, Quaternion.identity);
        Instantiate(HeadList[randomHead], ItemLocations[2].transform.position, Quaternion.identity);
        Instantiate(HealItem, ItemLocations[3].transform.position, Quaternion.identity);




        //for (int i = 0; i < ArmList.Length; i++) ArmList[i] = Resources.Load("Prefabs/Prefab" + i) as GameObject; 
        //Instantiate(ArmList[Random.Range(0, ArmList.Length)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (IsMenuActive == false)) 
        {
            Instantiate(ShopMenu, new Vector3(0, 0, 0), Quaternion.identity);
            IsMenuActive = true;
            Invoke("RefreshShopHud", 3);
        }
    }


    private void RandomizeOptions()
    {
        randomArm = Random.Range(0, ArmList.Count);
        randomLeg = Random.Range(0, LegList.Count);
        randomHead = Random.Range(0, HeadList.Count);
    }

    private void RefreshShopHud()
    {
        IsMenuActive = false;
        Debug.Log("Reset");
    }

    public void DestroyOption(int i)
    {
        switch (i)
        {

            case 0:
                ArmList[randomArm].gameObject.SetActive(false);
                break;
            case 1:
                LegList[randomLeg].gameObject.SetActive(false);
                break;
            case 2:
                HeadList[randomHead].gameObject.SetActive(false);
                break;
            case 3:
                HealItem.gameObject.SetActive(false);
                break;
        }
    }

}
