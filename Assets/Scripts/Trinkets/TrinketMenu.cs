using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrinketMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Button> OptionButtons;
    public List<TextMeshProUGUI> OptionTexts;

    public List<int> TotalTrinkets;
    public List<int> SelectedTrinket;
    public List<int> RandomInts;

    public bool active = false;

    public PlayerController PlayerController;
    public TrinketManager TrinketManager;
    
    public GameObject self;

    private void Start()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            RandomInts[i] = TotalTrinkets[Random.Range(0, TotalTrinkets.Count)];
            SelectedTrinket[i] = RandomInts[i];
            TotalTrinkets.Remove(RandomInts[i]);


 

            Debug.Log("Random Trinket #" + SelectedTrinket[i]);

            switch (SelectedTrinket[i])
            {
                case 0:
                    {
                        OptionTexts[i].text = $"Lizard Claw";
                        break;
                    }
                case 1:
                    {
                        OptionTexts[i].text = $"Bird Talon";
                        break;
                    }
                case 2:
                    {
                        OptionTexts[i].text = $"Tuft of Fur";
                        break;
                    }
                case 3:
                    {
                        OptionTexts[i].text = $"Feeding Frenzy";
                        break;
                    }
                case 4:
                    {
                        OptionTexts[i].text = $"Plump Mushroom";
                        break;
                    }
                case 5:
                    {
                        OptionTexts[i].text = $"Mule's Kick";
                        break;
                    }
                case 6:
                    {
                        OptionTexts[i].text = $"Hyena Jaw";
                        break;
                    }
                case 7:
                    {
                        OptionTexts[i].text = $"Scavenger";
                        break;
                    }
            }
        }
    }

    public void TrinketChoice(int i)
    {
        Debug.Log(SelectedTrinket[i]);

        switch (SelectedTrinket[i])
        {
            case 0:
                {
                    TrinketManager.LizardClaw();
                    break;
                }
            case 1:
                {
                    TrinketManager.BirdTalon();
                    break;
                }
            case 2:
                {
                    TrinketManager.TuftOfFur();
                    break;
                }
            case 3:
                {
                    TrinketManager.EnableFeedingFrenzy(); 
                    break;
                }
            case 4:
                {
                    TrinketManager.PlumpMushroom();
                    break;
                }
            case 5:
                {
                    TrinketManager.MulesKick();
                    break;
                }
            case 6:
                {
                    TrinketManager.HyenaJaw();
                    break;
                }
            case 7:
                {
                    TrinketManager.Scavenger();
                    break;
                }
        }
    }



    public void CloseMenu()
    {
        DestroyImmediate(self, true);
    }


}
