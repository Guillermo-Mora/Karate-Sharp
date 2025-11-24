using UnityEngine;
using System.Collections;
using System;

public class LevelTime : MonoBehaviour
{
    //Script que contiene el contador del tiempo del nivel

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private int maxTime = 500;
    private int time;
    
    public event Action OnTimeChanged;
    public event Action OnTimesUp;

    public int Time => time;

    private bool isRunning = true;

    private Coroutine timerCoroutine;

    void Start() {
        time = maxTime;
        OnTimeChanged?.Invoke();
        timerCoroutine = StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown() {
        while (time > 0 && isRunning) {
            if (playerStats.Died) {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(1f);
            time--;
            OnTimeChanged?.Invoke();
        }

        if (time <= 0)
        {
            OnTimesUp?.Invoke();
            OnTimeOver();
        }
    }

    private void OnTimeOver() {
        isRunning = false;
        StopCoroutine(timerCoroutine);
        playerStats.LoseLife();
    }

    public void restartTimerOnDeath() {
        isRunning = false;
        StopCoroutine(timerCoroutine);
        time = maxTime;
        OnTimeChanged?.Invoke();
        isRunning = true;
        timerCoroutine = StartCoroutine(TimerCountdown());
    }
}
