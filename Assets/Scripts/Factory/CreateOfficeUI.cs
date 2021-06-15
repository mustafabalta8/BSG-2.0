using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;

public class CreateOfficeUI : MonoBehaviour
{
    [SerializeField] GameObject officeUI;
    [SerializeField] GameObject officeUIPanel;

    [SerializeField] GameObject furnitureUI;
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
        CreateSingleOffice(5, 1000,0);
        CreateSingleOffice(15, 10,1);

    }
    void CreateSingleOffice(int capacity, int rent, int id)
    {
        GameObject newOffice = Instantiate(officeUI);
        newOffice.transform.SetParent(officeUIPanel.transform);
        Office office = newOffice.GetComponent<Office>();

        office.officeCapacity = capacity;
        office.rent = rent;
        office.id = id;
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
                CreateSingleFurniture("FlowersMaindoor02", 4, 800);
                CreateSingleFurniture("FlowersEmployees02", 6, 2500);
                CreateSingleFurniture("AirConditioning02", 5, 2000);
            }



        }

    }
    void CreateSingleFurniture(string FurName,int motivation,int price)
    {
        GameObject newFurniture = Instantiate(furnitureUI);
        newFurniture.transform.SetParent(furnitureUIPanel.transform);
        FurnitureUI furniture = newFurniture.GetComponent<FurnitureUI>();

        furniture.name = FurName;
        furniture.furnitureName = FurName;
        furniture.motivationIncrease = motivation;
        furniture.price = price;
        newFurniture.transform.Find("name").GetComponent<TextMeshProUGUI>().text = FurName;
        newFurniture.transform.Find("motivation").GetComponent<TextMeshProUGUI>().text = "Motivation:" + motivation;
        newFurniture.transform.Find("price").GetComponent<TextMeshProUGUI>().text = "Price:" + price;
    }
}
