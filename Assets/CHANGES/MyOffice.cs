using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class MyOffice : MonoBehaviour
{
    [SerializeField] GameObject Office01, Office02;
    [SerializeField] GameObject FurnitureHolder;
    [SerializeField] GameObject Flowers, PaintingCorner, Paintings, AirConditioning;
    [SerializeField] GameObject FlowersMaindoor02, FlowersEmployees02, AirConditioning02;

    string furnitureName;
    Vector3 Office2 = new Vector3(7.67f, -0.150444f, 0);
    DbManager dbManager;

    int officeVal;
    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        CheckStatus();
    }
    void CheckStatus()
    {
        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

        while (reader.Read())
        {
            officeVal = reader.GetInt32(0);
            if (officeVal == 0)
            {
                Office01.SetActive(true);
                Office02.SetActive(false);
            }
            else if (officeVal == 1)
            {
                Office01.SetActive(false);
                Office02.SetActive(true);
            }

           if (1 == reader.GetInt32(4))
            {
                ShowFurniture(0);
            }
           if (1 == reader.GetInt32(5))
            {
                ShowFurniture(1);
            }
           if (1 == reader.GetInt32(6))
            {
                ShowFurniture(2);
            }
           if (1 == reader.GetInt32(7))
            {
                ShowFurniture(3);
            }
            if (1 == reader.GetInt32(8))
            {
                ShowFurniture(4);
            }
            if (1 == reader.GetInt32(9))
            {
                ShowFurniture(5);
            }
            if (1 == reader.GetInt32(10))
            {
                ShowFurniture(6);
            }

        }
        dbManager.CloseConnection();
       
    }

    public void ShowFurniture(int value)
    {

        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);

       /* while (reader.Read())
        {
            officeVal = reader.GetInt32(0);
        }
        dbManager.CloseConnection();*/

        if (officeVal == 0)
            {
                switch (value)
                {
                    case 0:
                        Flowers.SetActive(true);
                        break;
                    case 1:
                        PaintingCorner.SetActive(true);
                        break;
                    case 2:
                        Paintings.SetActive(true);
                        break;
                    case 3:
                        AirConditioning.SetActive(true);
                        break;
                }
            }
            else if (officeVal == 1)
            {
                switch (value)
                {
                    case 4:
                        FlowersMaindoor02.SetActive(true);
                        break;
                    case 5:
                        FlowersEmployees02.SetActive(true);
                        break;
                    case 6:
                        AirConditioning02.SetActive(true);
                        break;
                }
            }
      

    }
}
