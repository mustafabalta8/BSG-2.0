using System.Collections;
using System.Collections.Generic;
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


    // deðiþikler

    public double elapsedTime;
    public Text TimeText;
    public int displayTime;

    [SerializeField] int secondsPerTurn = 10; // how many seconds do we want to pass before going to the next round?

    private void Start()
    {
        showTime(); // todo: show saved time from last save
     
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if((int)elapsedTime % secondsPerTurn == 0)
        {
            showTime();
        }

    }

    private void showTime()
    {
        displayTime = (int)(elapsedTime / secondsPerTurn) + 1;
        TimeText.text = displayTime.ToString();
    }
}