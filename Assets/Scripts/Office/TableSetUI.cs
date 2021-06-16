using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableSetUI : MonoBehaviour
{
    public string furnitureName;
    public int motivationIncrease;
    public int price;
    int amount;
    int capacity;

    DbManager dbManager;
    MoneyManager moneyManager;
    Company company;
    MyOffice myOffice;

    [SerializeField] Button BuyBtn;
    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        company = FindObjectOfType<Company>();
        myOffice = FindObjectOfType<MyOffice>();

        GetDataFromDB();

        transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount: " + amount;


    }
    private void GetDataFromDB()
    {
        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

        while (reader.Read())
        {
            amount = reader.GetInt32(3);
            capacity = reader.GetInt32(1);
        }

        dbManager.CloseConnection();
       // return amount;
    }
    public void BuyButton()
    {
        GetDataFromDB();
        if (moneyManager.money >= price && amount<capacity)
        {
            /*string query01 = "SELECT * FROM office";
            IDataReader reader = dbManager.ReadRecords(query01);

            while (reader.Read())
            {
                    amount = reader.GetInt32(3);
                    capacity = reader.GetInt32(1);
            }
            dbManager.CloseConnection();*/

                string query02 = string.Format("UPDATE office SET WorkTableSet='" + (amount+1) + "' ");
                dbManager.InsertRecords(query02);
                dbManager.CloseConnection();

                transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount: " + (amount+1);

                //moneyManager.changeMoney(-price, "Inventory");
                Debug.Log("workTable increased");

            myOffice.CheckStatus();
            company.ShowUpdateOnOfficeValues();
        }
        else
        {
            Debug.Log("You do not have enough capacity or money");
        }

    }


    public void SellButton()
    {
        if (amount > 0)
        {
            string query01 = "SELECT * FROM office";
            IDataReader reader = dbManager.ReadRecords(query01);

            while (reader.Read())
            {
                if (furnitureName == "WorkTable")
                {
                    amount = reader.GetInt32(3);
                }
                else if (furnitureName == "OfficeChair")
                {
                    amount = reader.GetInt32(4);
                }
                capacity = reader.GetInt32(1);

            }
            if (furnitureName == "WorkTable")
            {
                amount -= 1;
                string query02 = string.Format("UPDATE office SET workTable='" + amount + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                transform.Find("amount").GetComponent<TextMeshProUGUI>().text = amount.ToString();

                Debug.Log("workTable decreased");

            }
            else if (furnitureName == "OfficeChair")
            {
                amount -= 1;
                string query02 = string.Format("UPDATE office SET seat='" + amount + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);

                transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount: " + amount;

                Debug.Log("chair dec..");
            }

            dbManager.CloseConnection();

            moneyManager.changeMoney(price / 2, "Inventory");
            company.ShowUpdateOnOfficeValues();
        }

    }
}
