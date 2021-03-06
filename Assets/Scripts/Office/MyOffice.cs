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

    [Header("Emp. Table")]
    [SerializeField] GameObject TableSetHolder;
    [SerializeField] GameObject Table01, Table02, Table02Front;

    string furnitureName;
    Vector3 Office2 = new Vector3(7.67f, -0.150444f, 0);
    DbManager dbManager;
    Company company;
    int officeVal;
    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        company = FindObjectOfType<Company>();
        CheckStatus();
    }
    public void CheckStatus()
    {
        string query01 = "SELECT * FROM office";
        IDataReader reader = dbManager.ReadRecords(query01);
        int i=0;
        while (reader.Read())
        {
            officeVal = reader.GetInt32(0);
            if (officeVal == 0)
            {
                Office01.SetActive(true);
                Office02.SetActive(false);

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
                foreach (Transform child in TableSetHolder.transform)
                {
                    Destroy(child.gameObject);
                }

                for (i = 0; i < reader.GetInt32(3); i++) 
                {
                    GameObject EmpTable = Instantiate(Table01, company.Waypoints[i].position, Quaternion.identity);
                    EmpTable.transform.SetParent(TableSetHolder.transform);
                }


            }
            else if (officeVal == 1)
            {
                Office01.SetActive(false);
                Office02.SetActive(true);

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
                //-2.440444  -0.5104437  -> -1.93  Y
                //-2.670444  -0.7404437  -> -1.93  Y
                foreach (Transform child in TableSetHolder.transform)
                {
                    Destroy(child.gameObject);
                }

                for (i = 0; i < reader.GetInt32(3); i++)
                {
                    if(i < 9)
                    {
                        GameObject EmpTable = Instantiate(Table02Front, company.Waypoints[i].position, Quaternion.identity);
                        EmpTable.transform.position=new Vector3(EmpTable.transform.position.x, EmpTable.transform.position.y+(-1.93f) ,EmpTable.transform.position.z);
                        EmpTable.transform.SetParent(TableSetHolder.transform);
                    }else
                    {
                        GameObject EmpTable = Instantiate(Table02, company.Waypoints[i].position, Quaternion.identity);
                        EmpTable.transform.SetParent(TableSetHolder.transform);
                    }

                }
            }
                                      
        }

        dbManager.CloseConnection();
       
    }
    public void RentReaction(int officeID)
    {
        if (officeID == 1)
        {
            Flowers.SetActive(false); 
                PaintingCorner.SetActive(false); 
                Paintings.SetActive(false); 
                AirConditioning.SetActive(false);

        }
        if (officeID == 0)
        {
            FlowersMaindoor02.SetActive(false); 
                FlowersEmployees02.SetActive(false); 
                AirConditioning02.SetActive(false);
        }


    }


    public void ShowFurniture(int value)
    {


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
