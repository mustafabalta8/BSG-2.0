using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using UnityEngine.UI;
using System;

public class CreateOfficeUI : MonoBehaviour
{
    [SerializeField] GameObject officeUI;
    [SerializeField] GameObject officeUIPanel;

    [SerializeField] GameObject furnitureUI, tableUI;
    [SerializeField] GameObject furnitureUIPanel;

    [SerializeField] int totalOfficeNum;

    DbManager dbManager;
    int officeVal;
    // Start is called before the first frame update
    private void Awake()
    {
        dbManager = FindObjectOfType<DbManager>();
        CreateOffice();
    }
    void Start()
    {    
        CreateFurnitureUI();
    }

    void CreateOffice()
    {
        CreateSingleOffice(2, 1000,0);
        CreateSingleOffice(10, 5000,1);

    }
    void CreateSingleOffice(int capacity, int rent, int id)
    {
        //GameObject newOffice = Instantiate(officeUI);
        //newOffice.transform.SetParent(officeUIPanel.transform);

        GameObject newOffice = Instantiate(officeUI, officeUIPanel.transform, false);

        Office office = newOffice.GetComponent<Office>();

        office.officeCapacity = capacity;
        office.rent = rent;
        office.id = id;

        newOffice.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("officeImages/office"+id);
    }
    public void CreateFurnitureUI()
    {
        foreach (Transform child in furnitureUIPanel.transform)
        {
           Destroy(child.gameObject);
        }
        //CreateSingleFurniture("WorkTable", 2, 500);
        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

        while (reader.Read())
        {
            CreateTableUI("Work Table Set", 400);
            officeVal = reader.GetInt32(0);
            if (officeVal == 0)
            {
                CreateSingleFurniture("Flowers", 4, 800);
                CreateSingleFurniture("Painting-Art Corner", 6, 2500);
                CreateSingleFurniture("Paintings", 5, 2000);
                CreateSingleFurniture("Air Conditioning", 8, 4000);
            }
            else if (officeVal == 1)
            {
                CreateSingleFurniture("Flowers Maindoor", 4, 800);
                CreateSingleFurniture("Flowers Employees", 6, 2500);
                CreateSingleFurniture("Air Conditioning 2", 8, 4000);
            }

           

        }

    }
    void CreateSingleFurniture(string FurName,int motivation,int price)
    {
        // GameObject newFurniture = Instantiate(furnitureUI);
        // newFurniture.transform.SetParent(furnitureUIPanel.transform);
        GameObject newFurniture = Instantiate(furnitureUI, furnitureUIPanel.transform, false);
        FurnitureUI furniture = newFurniture.GetComponent<FurnitureUI>();

        furniture.name = FurName;
        furniture.furnitureName = FurName;
        furniture.motivationIncrease = motivation;
        furniture.price = price;
        newFurniture.transform.Find("name").GetComponent<TextMeshProUGUI>().text = FurName;
        newFurniture.transform.Find("motivation").GetComponent<TextMeshProUGUI>().text = "Motivation:" + motivation;
        newFurniture.transform.Find("price").GetComponent<TextMeshProUGUI>().text = "Price:" + price;

        newFurniture.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("furnitures/"+FurName);
    }

    void CreateTableUI(string FurName, int price)
    {
        //GameObject newFurniture = Instantiate(tableUI);
        // newFurniture.transform.SetParent(furnitureUIPanel.transform);
        GameObject newFurniture = Instantiate(tableUI, furnitureUIPanel.transform, false);
        TableSetUI furniture = newFurniture.GetComponent<TableSetUI>();

        furniture.name = FurName;
        furniture.furnitureName = FurName;
        furniture.price = price;

        newFurniture.transform.Find("name").GetComponent<TextMeshProUGUI>().text = FurName;
        newFurniture.transform.Find("price").GetComponent<TextMeshProUGUI>().text = "Price:" + price;
        newFurniture.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("furnitures/table");
    }
}
