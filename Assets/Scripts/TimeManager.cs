using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //TODO: save time to be continued by on the next launch
    //TODO: add time speed change Time.timeScale
    //TODO: progress bar

    // hour->day->week->month


    public double elapsedTime;
    public Text TimeText;
    public int displayTime = 1;

    public int secondsPerTurn = 60; // how many seconds do we want to pass before going to the next round?

    [SerializeField] Button startStopButton;
    [SerializeField] TextMeshProUGUI startStopButtonText;

    [SerializeField] Sprite OffSprite;
    [SerializeField] Sprite OnSprite;

    public bool timeStopped = false;
    public int timeScale = 1;
    int clockTime = 9;

    private void Start()
    {
        StartCoroutine(startClock());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    IEnumerator startClock() 
    { 
        if (!timeStopped)
        {
            TimeText.text = $"{clockTime}:00";

            if (clockTime == 17)
            {
                clockTime = 9;
                TimeText.text = $"{clockTime}:00";
                displayTime += 1;
            }

            clockTime += 1;

            yield return new WaitForSeconds(secondsPerTurn/7.9f);

            StartCoroutine(startClock());
        }
    }

    public void startStopTime()
    {
        timeStopped = !timeStopped;
        if (timeStopped)
        {
            Time.timeScale = 0;
            startStopButton.image.sprite = OnSprite;
        }
        else
        {
            startStopButton.image.sprite = OffSprite;
            Time.timeScale = 1.0f;
        }
    }

    public void speedUp(int timeScale)
    {
        this.timeScale = timeScale;

        if(timeScale == 2)
        {
            Time.timeScale = 2.0f;
        }
        else if(timeScale == 5)
        {
            Time.timeScale = 5.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}