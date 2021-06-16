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


        CheckStatus();
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
                string query02 = string.Format("UPDATE office SET \"Flowers Maindoor\" = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(3);
            }
            else if (furnitureName == "FlowersEmployees02")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET \"Flowers Employees\" = '" + available + "' WHERE id ='1'");
                dbManager.InsertRecords(query02);
                //myOffice.ShowFurniture(3);
            }
            else if (furnitureName == "AirConditioning02")
            {
                available = 1;
                string query02 = string.Format("UPDATE office SET \"Air Conditioning 2\"= '" + available + "' WHERE id ='1'");
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



   


}
