using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //todo: save time to be continued by on the next launch

    public double elapsedTime;
    public Text TimeText;
    public int displayTime;

    private void Start()
    {
        showTime(); // todo: show saved time from last save
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if((int)elapsedTime % 10 == 0)
        {
            showTime();
        }

    }

    private void showTime()
    {
        displayTime = (int)(elapsedTime / 10) + 1;
        TimeText.text = displayTime.ToString();
    }
}