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

    double actualTime;

    void Start()
    {
        
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;
        actualTime = (elapsedTime / 10);
        displayTime = (int)actualTime+1;
        TimeText.text = displayTime.ToString();
    }
}
