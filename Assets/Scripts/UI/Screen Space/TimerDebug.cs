using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private PostRunSummaryController pRSController;

    private void Start()
    {
        pRSController = PostRunSummaryController.Instance;
    }

    private void OnEnable()
    {
        PostRunSummaryController.OnTimerUpdate += UpdateTimer;
    }

    private void OnDisable()
    {
        PostRunSummaryController.OnTimerUpdate -= UpdateTimer;
    }

    private void UpdateTimer()
    {
        float seconds = Mathf.Round(pRSController.currentTime % 60);
        float minutes = Mathf.Round(pRSController.currentTime/60) % 60;
        float hours = Mathf.Round(pRSController.currentTime/3600) % 24;
        timerText.text = hours + ":" + minutes + ":" + seconds;
    }
}
