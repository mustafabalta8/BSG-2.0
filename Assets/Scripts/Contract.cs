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
    

    double employeePower;
    double whenItShouldBeDone = 0;
    //necesarry contract object variables

    
    // define necesarry scripts to be used later
    Employee employee; 
    TimeManager time;
    MoneyManager moneyManager;
    // define necesarry scripts to be used later


    private void Start()
    {
        // define necesarry scripts to be used later
        employee = FindObjectOfType<Employee>();
        time = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        // define necesarry scripts to be used later

        //TODO: change to company structure, now it only reads ONE employee's power
        employeePower = employee.employeePower; //get employee power
        acceptedTime = time.displayTime; // get in-game time
    }

    private void Update()
    {
        checkContractStatus(whenItShouldBeDone); //check contract status continuously
    }


    public void acceptContract() //button trigger method to accept a contract
    {
        int requiredTimeForWorker = (int)((requiredPower / employeePower) + 0.5f); //get the required time a worker should take to finished the given job
        if (requiredTimeForWorker <= deadLine) //if the worker DOES NOT takes longer than the deadline, go ahead and accept the contract
        {
            double timeNow = time.displayTime;
            whenItShouldBeDone = timeNow + requiredTimeForWorker;
            Debug.Log(deadLine + " hafta sürmesi gereken iþi " + employeePower + " gücünüz ile " + requiredTimeForWorker + " haftada tamamlamak üzere alýndýnýz. Bu iþ "+ whenItShouldBeDone +". hafta bitecektir");
        }
        else //if the worker DOES takes longer than the deadline, refuse contract
        {
            Debug.Log("Gücünüz, " + deadLine + " hafta sürecek iþi bitirmek için yetersiz. (Bu iþ gücünüz ile " + requiredTimeForWorker + " sürecektir)");
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

                Debug.Log("Ýþ tamamlandý");
                completed = true; //complete the contract
                return;
            }
        }

    }
}
