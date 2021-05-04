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
            amount = reader.GetInt32(3);
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
            amount = reader.GetInt32(3);
            capacity = reader.GetInt32(1);
        }
        
        if (furnitureName== "WorkTable"&& amount<=capacity)
        {
            amount += 1;
            string query02 = string.Format("UPDATE office SET workTable='" + amount + "' WHERE id ='1'");
            dbManager.InsertRecords(query02);
            dbManager.CloseConnection();

            transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;
            transform.Find("price").GetComponent<TextMeshProUGUI>().text = "Price:" + price;
            Debug.Log("workTable increased");
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
            amount = reader.GetInt32(3);
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

        dbManager.CloseConnection();

        moneyManager.changeMoney(price/2);
        company.ShowUpdateOnOfficeValues();
    }


}
