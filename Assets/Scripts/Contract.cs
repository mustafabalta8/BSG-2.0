using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract : MonoBehaviour
{
    //necesarry contract object variables
    [SerializeField] double requiredPower = 10;
    [SerializeField] double acceptedTime;
    [SerializeField] double deadLine = 10;
    [SerializeField] double contractValue = 100;
    [SerializeField] bool completed = false;

    [Header("Presented Variables")]
    public string platform;
    public string sofType;
   // public string companyName;
    public int duration;
    public int offer;
    public int code;
    public int art;
    public int design;
    public int ContID;

    double employeePower;
    double whenItShouldBeDone = 0;
    //necesarry contract object variables

    
    // define necesarry scripts to be used later
    Employee employee; 
    TimeManager time;
    MoneyManager moneyManager;
    AcceptContract acceptContract0;
    // define necesarry scripts to be used later


    private void Start()
    {
        gameObject.name = ContID.ToString();
        // define necesarry scripts to be used later
       
        time = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        acceptContract0 = FindObjectOfType<AcceptContract>();
        // define necesarry scripts to be used later

        //TODO: change to company structure, now it only reads ONE employee's power
       
        acceptedTime = time.displayTime; // get in-game time
    }

    public void SelectedContract()
    {
        acceptContract0.platform = platform;
        acceptContract0.sofType = sofType;
        acceptContract0.duration = duration;
        acceptContract0.offer = offer;
        acceptContract0.code = code;
        acceptContract0.art = art;
        acceptContract0.design = design;
        acceptContract0.ContID = ContID;
    }
    
}
