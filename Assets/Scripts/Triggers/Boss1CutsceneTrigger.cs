using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boss1CutsceneTrigger : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject gate;

    // private Collider cutsceneTrigger;

    public static Action Boss1Cutscene;

    void Awake()
    {
        // cutsceneTrigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            Boss1Cutscene?.Invoke();
            StartCoroutine(CloseGate());
            Invoke("SpawnBoss", 3f);
            PlayerController.Instance.DisableAllDefaultControls();
            Invoke("ResumePlayerControls", 4f);
        }
    }

    private void SpawnBoss()
    {
        boss.SetActive(true);
    }

    private IEnumerator CloseGate()
    {
        float timer = 0;
        while (timer < 0.5)
        {
            timer += Time.deltaTime;
            gate.transform.position = new Vector3(gate.transform.position.x, Mathf.Lerp(8, 0, timer / 0.5f), gate.transform.position.z);
            yield return null;
        }
        yield return null;
    }

    private void ResumePlayerControls()
    {
        PlayerController.Instance.EnableAllDefaultControls();
    }
}
