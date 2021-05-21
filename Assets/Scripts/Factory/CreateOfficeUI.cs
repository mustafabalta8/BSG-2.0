using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateOfficeUI : MonoBehaviour
{
    [SerializeField] GameObject officeUI;
    [SerializeField] GameObject officeUIPanel;

    [SerializeField] GameObject furnitureUI;
    [SerializeField] GameObject furnitureUIPanel;

    [SerializeField] int totalOfficeNum;
    // Start is called before the first frame update
    void Start()
    {
        CreateFurnitureUI();
        CreateOffice();
    }

    void CreateOffice()
    {

        for (int i=1; i <= totalOfficeNum; i++)
        {
            GameObject newOffice = Instantiate(officeUI);
            newOffice.transform.SetParent(officeUIPanel.transform);
            Office office = newOffice.GetComponent<Office>();

            office.officeCapacity = 5 * i;
            office.rent = 1000 * i;
        }
        
    }
    void CreateFurnitureUI()
    {
        //  WORK TABLE CREATION
        CreateSingleFurniture("WorkTable", 2, 120);
        CreateSingleFurniture("OfficeChair", 1, 80);


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
