using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data;
using System;

public class Office : MonoBehaviour
{

    public int officeCapacity;
    public int rent;

    Company company;
    DbManager dbManager;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.Find("capacity").GetComponent<TextMeshProUGUI>().text = "Office Capacity:"+officeCapacity.ToString();
        this.transform.Find("rent").GetComponent<TextMeshProUGUI>().text = "Monthly Rent:"+rent.ToString();
        transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop");

        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();
    }
    public void Rent()
    {
        company.OurOfficeCapacity = officeCapacity;
        company.OurOfficeRent = rent;

        //string query = string.Format("UPDATE office SET rent='" + rent + "',capacity='"+officeCapacity+"' WHERE id ='1'");
        string query = string.Format("UPDATE office SET rent='{0}',capacity='{1}' WHERE id ='1'", rent, officeCapacity);

        dbManager.InsertRecords(query);
            dbManager.CloseConnection();
        Debug.Log("Rented");

        company.ShowUpdateOnOfficeValues();
           // transform.Find("amount").GetComponent<TextMeshProUGUI>().text = "Amount:" + amount;
           // transform.Find("price").GetComponent<TextMeshProUGUI>().text = "Price:" + price;       

    }


    
}
