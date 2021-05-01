using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //todo: save time to be continued by on the next launch

    public float elapsedTime = 1;
    public Text TimeText;
    public int displayTime;
    double actualTime = 1;
    News news;
    private static int kills = 0;

    public static int Kills //how to reach Kills from another class. timeManager.Kills error??
    {
        get
        {
            return kills;
        }
        set
        {
            kills = value;
            OnVarChange();
        }
    }
    void Start()
    {

        news = FindObjectOfType<News>();
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;
        actualTime = (elapsedTime / 10);
        displayTime = (int)actualTime + 1;
        if ((int)elapsedTime % 5 == 0)
        {
            Debug.Log("actual time:" + actualTime);
            actualTime++;
            kills++;
            elapsedTime = 1;
            news.DestroyNews();
            news.CreateNews();
        }


        displayTime = (int)actualTime;
        TimeText.text = displayTime.ToString();
    }

    public static void OnVarChange()
    {

        Debug.Log("Kills:" + kills);
    }
}
