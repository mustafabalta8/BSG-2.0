using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] Text moneyText; //define text field to show the amount of money

    Company company;
    TimeManager time;
    DbManager dbManager;

    public int money; //initial money

    void Start()
    {
        company = FindObjectOfType<Company>(); //get the company script
        time = FindObjectOfType<TimeManager>(); //get the time script
        dbManager = FindObjectOfType<DbManager>(); //get the db manager script

        money = getBalance();

        moneyText.text = "$" + money.ToString(); // show the money at startup
    }

    // Update is called once per frame
    void Update()
    {
       /* if((time.displayTime) % 3 == 0)
        {
            if (money > company.totalSalary)
                changeMoney(-(company.totalSalary));
            else
            {
                //Debug.Log("bankrupt!");
            }
        }*/
    }

    public void changeMoney(int amount, string type) //method to dynamicly change amount of money, takes a integer as parameter which adds up to the current amount of money, then updates the current money text field that the user sees
    {
        saveTransaction(amount, type);

        money = money + amount; //add parameter value to the money amount
        moneyText.text = "$" + money.ToString(); //update text field

        company.balance = money;
        company.saveCompanyData();
    }

    public void saveTransaction(int transaction, string type)
    {
        string query = $"INSERT INTO bank_transactions VALUES ({money},{transaction},\"{type}\",{time.displayTime})";
        dbManager.ReadRecords(query);
        dbManager.CloseConnection();
    }

    public int getBalance()
    {
        int balance = 0;

        string query = "SELECT * FROM company";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            balance = reader.GetInt32(1);
        }

        dbManager.CloseConnection();

        return balance;
    }
}
