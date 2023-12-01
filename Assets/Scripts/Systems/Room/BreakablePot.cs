using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakablePot : MonoBehaviour
{
    [SerializeField] private GameObject boneDropS;
    [SerializeField] private GameObject boneDropM;
    [SerializeField] private GameObject boneDropL;
    private float randomInt;
    // Start is called before the first frame update
    private void Start()
    {
        randomInt = Random.Range(0, 4);
    }
    public void SpawnBones()
    {
        for (int i = 0; i < randomInt; i++)
        {
            int randomDrop = Random.Range(1, 3);
            switch (randomDrop)
            {
                case 1:
                    {
                        Instantiate(boneDropS, transform.position, Quaternion.identity);
                        break;
                    }
                case 2:
                    {
                        Instantiate(boneDropM, transform.position, Quaternion.identity);
                        break;
                    }
                case 3:
                    {
                        Instantiate(boneDropL, transform.position, Quaternion.identity);
                        break;
                    }
                    
            }
            
        }
        
    }

}
