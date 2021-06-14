using System;
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

    [SerializeField] TMP_Text dayMonthText;
    [SerializeField] TMP_Text dateDetailsText;

    public bool timeStopped = false;
    public int timeScale = 1;
    int clockTime = 9;

    public DateTime date;
    DateTime startDate = new DateTime(2021, 1, 1);

    NewDay newDayController;

    private void Start()
    {
        date = new DateTime(2021, 1, 1);

        dateDetailsText.text = $"First day.";
        dayMonthText.text = date.ToString("dd/MM/yyyy");

        newDayController = FindObjectOfType<NewDay>();

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
                newDayController.newDay("New day");
                clockTime = 9;
                TimeText.text = $"{clockTime}:00";
                displayTime += 1;
                date = date.AddDays(1);
                calculateDate();
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
            Time.timeScale = 20.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public string calculateDate()
    {
        dayMonthText.text = date.ToString("dd/MM/yyyy");

        int DAYS_IN_WEEK = 7;
        int number_of_days = (int)(date - startDate).TotalDays;

        int year, week, days, month;

        // Assume that years
        // is of 365 days
        year = number_of_days / 365;
        week = (number_of_days % 365) /
                DAYS_IN_WEEK;
        days = (number_of_days % 365) %
                DAYS_IN_WEEK;
        month = GetMonthDifference(date, startDate);

        dateDetailsText.text = $"Y: {year}, M: {month}, W: {week}, D: {days}";

        return $"Y: {year}, M: {month}, W: {week}, D: {days+2}";
    }

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }

}