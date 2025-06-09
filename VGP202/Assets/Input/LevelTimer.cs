using System;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public TMP_Text currentTimeText;

    private void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString("D2");
        //currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public void startTimer()
    {
        timerActive = true;
    }
    public void stopTimer()
    {
        timerActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            startTimer();
        }
    }
  
}
