using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeTimer : MonoBehaviour
{
    [SerializeField] private Text timer;
    [SerializeField] private GameObject timerBG;

    [SerializeField] private float timeGiven = 5f;

    public static EscapeTimer inst;

    private Coroutine timerCoroutine = null;

    private void Awake()
    {
        if (inst != null)
        {
            Destroy(this);
        }

        if (inst == null)
        {
            inst = this;
        }

        StopTimer();
    }

    public void StartTimer()
    {
        StopTimer();
        timerBG.SetActive(true);
        timerCoroutine = StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float time = timeGiven;

        while (time > 0)
        {
            time -= Time.deltaTime;

            timer.text = Mathf.Round(time).ToString();
            yield return null;
        }
        WorldPoints.inst.SpawnPlayer();
    }

    public void StopTimer()
    {
        timerBG.SetActive(false);
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = null;
    }
}
