using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int money = 1000000; //initial money
    [SerializeField] Text moneyText; //define text field to show the amount of money

    Company company;
    TimeManager time;
    void Start()
    {
        company = FindObjectOfType<Company>(); //get the company script
        time = FindObjectOfType<TimeManager>(); //get the time script

        moneyText.text = "$" + money.ToString(); // show the money at startup
    }

    // Update is called once per frame
    void Update()
    {
        if((time.displayTime) % 3 == 0)
        {
            if (money > company.totalSalary)
                changeMoney(-(company.totalSalary));
            else
            {
                //Debug.Log("bankrupt!");
            }
        }
    }

    public void changeMoney(int amount) //method to dynamicly change amount of money, takes a integer as parameter which adds up to the current amount of money, then updates the current money text field that the user sees
    {
        money = money + amount; //add parameter value to the money amount
        moneyText.text = "$" + money.ToString(); //update text field
    }
}
