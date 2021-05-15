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
    }
    private void Update()
    {
       // checkContractStatus(whenItShouldBeDone); //check contract status continuously
    }


    public void acceptContract() //button trigger method to accept a contract
    {
        int requiredTimeForWorker = (int)((requiredPower / employeePower) + 0.5f); //get the required time a worker should take to finished the given job
        if (requiredTimeForWorker <= deadLine) //if the worker DOES NOT takes longer than the deadline, go ahead and accept the contract
        {
            double timeNow = time.displayTime;
            whenItShouldBeDone = timeNow + requiredTimeForWorker;
            Debug.Log(deadLine + " hafta s�rmesi gereken i�i " + employeePower + " g�c�n�z ile " + requiredTimeForWorker + " haftada tamamlamak �zere al�nd�n�z. Bu i� "+ whenItShouldBeDone +". hafta bitecektir");
        }
        else //if the worker DOES takes longer than the deadline, refuse contract
        {
            Debug.Log("G�c�n�z, " + deadLine + " hafta s�recek i�i bitirmek i�in yetersiz. (Bu i� g�c�n�z ile " + requiredTimeForWorker + " s�recektir)");
        }
    }
    
    public void checkContractStatus(double whenItShouldBeDone) //method to check contract status, runs only if there is a contract taken at the moment
    {
        double timeNow = time.displayTime; //get in-game time

        if (!completed) //if the contract is NOT completed
        {
            if (timeNow == whenItShouldBeDone) // if the time that the contract should be completed matches the current in-game time, than finish contract
            {
                moneyManager.changeMoney((int)contractValue); //add the contract amount to the in-game money through the money managers' change money method

                Debug.Log("�� tamamland�");
                completed = true; //complete the contract
                return;
            }
        }

    }
}
