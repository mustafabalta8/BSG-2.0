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
    public int id;

    Company company;
    DbManager dbManager;
    MyOffice myOffice;
    CreateOfficeUI createOfficeUI;
    // Start is called before the first frame update
    void Start()
    {
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();
        myOffice = FindObjectOfType<MyOffice>();
        createOfficeUI = FindObjectOfType<CreateOfficeUI>();

        this.transform.Find("capacity").GetComponent<TextMeshProUGUI>().text = ""+officeCapacity.ToString();
        this.transform.Find("rent").GetComponent<TextMeshProUGUI>().text = ""+rent.ToString();
        transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop");


    }
    public void Rent()
    {
        company.OurOfficeCapacity = officeCapacity;
        company.OurOfficeRent = rent;

        //string query = string.Format("UPDATE office SET rent='" + rent + "',capacity='"+officeCapacity+"' WHERE id ='1'");
        string query = string.Format("UPDATE office SET id='{0}',capacity='{1}',rent='{2}' ", id, officeCapacity,rent );

        dbManager.InsertRecords(query);
        dbManager.CloseConnection();


        Debug.Log("Rented");
        company.ShowUpdateOnOfficeValues();
        myOffice.RentReaction(id);
        myOffice.CheckStatus();
        createOfficeUI.CreateFurnitureUI();
        //company.EmployeeAnimation();




    }


    
}
