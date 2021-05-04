using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;

public class FurnitureUI : MonoBehaviour
{
    public string furnitureName;
    public int motivationIncrease;
    public int price;
    int amount;
    int capacity;
    
    DbManager dbManager;
    MoneyManager moneyManager;
    Company company;
    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        company = FindObjectOfType<Company>();

        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

        while (reader.Read())
        {
            if (furnitureName == "WorkTable")
            {
                amount = reader.GetInt32(3);
            }else if (furnitureName == "OfficeChair")
            {
                amount = reader.GetInt32(4);
            }
            
        }
        dbManager.CloseConnection();
        transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;
    }
    public void BuyButton()
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
        
        if (furnitureName== "WorkTable"&& amount<=capacity)
        {
            amount += 1;
            string query02 = string.Format("UPDATE office SET workTable='" + amount + "' WHERE id ='1'");
            dbManager.InsertRecords(query02);

            transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;
            
            Debug.Log("workTable increased");
        }else if(furnitureName == "OfficeChair" && amount <= capacity)
        {
            amount += 1;
            string query02 = string.Format("UPDATE office SET seat='" + amount + "' WHERE id ='1'");
            dbManager.InsertRecords(query02);

            transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;

            Debug.Log("chair increased");
        }

        dbManager.CloseConnection();

        moneyManager.changeMoney(-price);
        company.ShowUpdateOnOfficeValues();
    }

    public void SellButton()
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
            transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;

            Debug.Log("workTable decreased");

        }
        else if (furnitureName == "OfficeChair" && amount <= capacity)
        {
            amount -= 1;
            string query02 = string.Format("UPDATE office SET seat='" + amount + "' WHERE id ='1'");
            dbManager.InsertRecords(query02);

            transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;

            Debug.Log("chair dec..");
        }

        dbManager.CloseConnection();

        moneyManager.changeMoney(price/2);
        company.ShowUpdateOnOfficeValues();
    }


}
