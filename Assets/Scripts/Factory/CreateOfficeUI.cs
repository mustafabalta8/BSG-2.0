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
            GameObject newDesk = Instantiate(furnitureUI);
            newDesk.transform.SetParent(furnitureUIPanel.transform);

            FurnitureUI furniture = newDesk.GetComponent<FurnitureUI>();
            furniture.name = "WorkTable";
            furniture.furnitureName = "WorkTable";
            furniture.motivationIncrease = 2;
            furniture.price = 150;
            newDesk.transform.Find("name").GetComponent<TextMeshProUGUI>().text = "Work Table";
            newDesk.transform.Find("motivation").GetComponent<TextMeshProUGUI>().text = "Motivation:"+2;
        



    }
}
