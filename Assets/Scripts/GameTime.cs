using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTime : MonoBehaviour
{
    public float timeLeft;
    public bool timerOn = false;

    TMP_Text timerText;

    public UnityEvent onTime;

    private void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }

            else
            {
                timeLeft = 0;
                timerOn = false;
                onTime.Invoke();
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{00}:{1:00}", minutes, seconds);
    }
}
