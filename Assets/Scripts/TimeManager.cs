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
    public int clockTime;

    public int turn;
    private void Start()
    {
        showTime(); // todo: show saved time from last save

    }

    private void Update()
    {
        if (!timeStopped)
        {
            elapsedTime += Time.deltaTime;

            //if ((int)elapsedTime % secondsPerTurn == 0)
            //{
            //showTime();
            //}

            showTime();

        }
    }

    private void showTime()
    {
        displayTime = (int)(elapsedTime / secondsPerTurn) + 1;

        if (clockTime < 24)
        {
            clockTime = ((int)elapsedTime / 4) + 9;
        }
        else
        {
            clockTime = 0;
        }


        TimeText.text = $"{clockTime}:00";


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