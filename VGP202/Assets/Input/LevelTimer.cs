using System;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public TMP_Text currentTimeText;
    public int yourTime;

    public TMP_Text yourScoreText;
    public TMP_Text highScoreText;

    private void Start()
    {
        currentTime = 0;

        if (GameMaster.Instance != null)
        {
            GameMaster.Instance.OnPlayerSpawned += HandlePlayerSpawned;
        }
    }

    private void HandlePlayerSpawned(PlayerController pc)
    {
        startTimer();
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

    public string getTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString("D2");
    }
    public void startTimer()
    {
        timerActive = true;
    }
    public void stopTimer()
    {
        timerActive = false;
        //string finalTime = getTime();

        yourTime = Mathf.RoundToInt(currentTime);

        highScoreUpdate();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           // Debug.Log("Hit Player");
            startTimer();
        }
    }

    private void OnDestroy()
    {
        if (GameMaster.Instance != null)
        {
            GameMaster.Instance.OnPlayerSpawned -= HandlePlayerSpawned;
        }
    }

    public void highScoreUpdate()
    {
        int savedHighScore = PlayerPrefs.GetInt("SavedHighScore", int.MaxValue);

        if (yourTime > savedHighScore)
        {
            PlayerPrefs.SetInt("SavedHighScore", yourTime);
            savedHighScore = yourTime;
        }

        PlayerPrefs.Save();
        TimeSpan yourTimeSpan = TimeSpan.FromSeconds(yourTime);
        TimeSpan bestTimeSpan = TimeSpan.FromSeconds(savedHighScore);

        yourScoreText.text = "" + yourTimeSpan.Minutes.ToString() + ":" + yourTimeSpan.Seconds.ToString("D2");
        highScoreText.text = "" + bestTimeSpan.Minutes.ToString() + ":" + bestTimeSpan.Seconds.ToString("D2");
    }
}
