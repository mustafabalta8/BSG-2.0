using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureUI : MonoBehaviour
{
    public string furnitureName;
    public int motivationIncrease;
    public int price;
    int amount, available;
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

        // amount= GetDataFromDB(amount);

        CheckStatus();
      // transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount: " + amount;


    }
    private int GetDataFromDB(int amount01)
    {
        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

        while (reader.Read())
        {
            if (furnitureName == "WorkTable")
            {
                amount01 = reader.GetInt32(3);
            }
            else if (furnitureName == "OfficeChair")
            {
                amount01 = reader.GetInt32(4);
            }

        }
        
        dbManager.CloseConnection();
        return amount01;
    }
    public void BuyButton()
    {
        if ((moneyManager.money - price) >= 0)
        {
            string query01 = "SELECT * FROM office";
            IDataReader reader = dbManager.ReadRecords(query01);

            while (reader.Read())
            {
                if (furnitureName == "WorkTable")
                {
                    amount = reader.GetInt32(3);
                }
                /*else if (furnitureName == "OfficeChair")
                {
                    amount = reader.GetInt32(4);
                }*/
                capacity = reader.GetInt32(1);
            }

            if (furnitureName == "WorkTable" && amount <= capacity-1)
            {
                amount += 1;
                string query02 = string.Format("UPDATE office SET workTable='" + amount + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);

                transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount: " + amount;
                moneyManager.changeMoney(-price, "Inventory");
                Debug.Log("workTable increased");

            }
            /*else if (furnitureName == "OfficeChair" && amount <= capacity)
            {
                amount += 1;
                string query02 = string.Format("UPDATE office SET seat='" + amount + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);

                transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount: " + amount;

                Debug.Log("chair increased");
            }*/

            dbManager.CloseConnection();

            
            company.ShowUpdateOnOfficeValues();
        }
        
    }
    public void Buy()
    {
        if (moneyManager.money >= price)
        {
            if (furnitureName == "Flowers")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET Flowers= '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                

            }
            else if (furnitureName == "Painting-Art Corner")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET \"Painting-Art Corner\" = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
               // myOffice.ShowFurniture(1);
            }
            else if (furnitureName == "Paintings")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET Paintings= '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(2);
            }
            else if (furnitureName == "Air Conditioning")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET \"Air Conditioning\" = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(3);
            }
            else if (furnitureName == "FlowersMaindoor02")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET \"FlowersMaindoor02\" = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(3);
            }
            else if (furnitureName == "FlowersEmployees02")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET FlowersEmployees02 = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(3);
            }
            else if (furnitureName == "AirConditioning02")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET AirConditioning02 = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(3);
            }

            dbManager.CloseConnection();
            moneyManager.changeMoney(-price, "Furniture");
        }  
        
        CheckStatus();
    }
    void CheckStatus()
    {
        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

        while (reader.Read())
        {
            if (furnitureName == "Flowers")
            {
                available = reader.GetInt32(4);
                if(available==1)
                    myOffice.ShowFurniture(0);
            }
            else if (furnitureName == "Painting-Art Corner")
            {
                available = reader.GetInt32(5);
                if (available == 1)
                    myOffice.ShowFurniture(1);
            }
            else if (furnitureName == "Paintings")
            {
                available = reader.GetInt32(6);
                if (available == 1)
                    myOffice.ShowFurniture(2);
            }
            else if (furnitureName == "Air Conditioning")
            {
                available = reader.GetInt32(7);
                if (available == 1)
                    myOffice.ShowFurniture(3);
            }
            else if (furnitureName == "FlowersMaindoor02")
            {
                available = reader.GetInt32(8);
                if (available == 1)
                    myOffice.ShowFurniture(0);
            }
            else if (furnitureName == "FlowersEmployees02")
            {
                available = reader.GetInt32(9);
                if (available == 1)
                    myOffice.ShowFurniture(1);
            }
            else if (furnitureName == "AirConditioning02")
            {
                available = reader.GetInt32(10);
                if (available == 1)
                    myOffice.ShowFurniture(2);
            }

        }
        dbManager.CloseConnection();
        if (available == 1)
        {
            BuyBtn.interactable = false;
            BuyBtn.GetComponent<Image>().color = Color.red;
            BuyBtn.transform.Find("Text").GetComponent<Text>().text = "SOLD";
            
        }
    }



    public void SellButton()
    {
        if(amount > 0)
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

            moneyManager.changeMoney(price / 2,"Inventory");
            company.ShowUpdateOnOfficeValues();
        }
        
    }


}
