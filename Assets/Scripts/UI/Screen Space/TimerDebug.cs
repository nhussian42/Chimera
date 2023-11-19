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
        timerText.text = pRSController.currentTime.ToString();
    }
}
