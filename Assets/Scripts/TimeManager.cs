using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //todo: save time to be continued by on the next launch

    //3.05.2021 newsfeed codebase update
    //why do we use this script, which has no other function but keep track of time to initiate create and destroy news objects?
    //basically everything that has anything to do with the newsfeed logic is moved into the news script to ease debugging and maintenance in the future
    //i believe that the sole reason of this scripts' existence should remain with time related issues


    //TODO: add time speed change Time.timeScale
    //TODO: progress bar

    public double elapsedTime;
    public Text TimeText;
    public int displayTime = 1;

    public int secondsPerTurn = 60; // how many seconds do we want to pass before going to the next round?

    [SerializeField] Button startStopButton;
    [SerializeField] TextMeshProUGUI startStopButtonText;

    [SerializeField] Sprite OffSprite;
    [SerializeField] Sprite OnSprite;


    public Text timeScaleText;

    public bool timeStopped = false;
    private int timeScale = 1;

    private void Start()
    {
        showTime(); // todo: show saved time from last save
    }

    private void Update()
    {
        if (!timeStopped)
        {
            elapsedTime += Time.deltaTime;

            if ((int)elapsedTime % secondsPerTurn == 0)
            {
                showTime();
            }
        }
    }

    private void showTime()
    {
        displayTime = (int)(elapsedTime / secondsPerTurn) + 1;
        TimeText.text = displayTime.ToString();
        timeScaleText.text = $"{timeScale}x";
    }

    public void startStopTime()
    {
        timeStopped = !timeStopped;
        if (timeStopped)
        {
            startStopButton.image.sprite = OnSprite;
        }
        else
        {
            startStopButton.image.sprite = OffSprite;
            Time.timeScale = 1.0f;

            timeScaleText.text = $"{Time.timeScale}x";
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

        timeScaleText.text = $"{timeScale}x";
    }
}