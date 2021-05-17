using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.Linq;
using TMPro;

public class Employee : MonoBehaviour
{
    public int employeeId;
    public string employeeName;
    public int employeeSalary;
    public int employeePower;

    public int code, art, design;

    [SerializeField] Button Hire;  // connected with the HireButton via prefab
    [SerializeField] TextMeshProUGUI ButttonText;

    Company company;
    DbManager dbManager;
    AcceptContract acceptContract;

    public void Start()
    {
        acceptContract = FindObjectOfType<AcceptContract>();
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();


    }

    public void hireEmployee()
    {
        company.AddEmployeeToCompany(this);
        Destroy(gameObject);
    }

    public void fireEmployee()
    {
        company.FireEmployee(this);
        Destroy(gameObject);
    }
    public void assignEmployee()
    {
        if (ButttonText.GetComponent<TextMeshProUGUI>().text == "Assign")
        {
            string query = string.Format("UPDATE employees SET busy='1' WHERE employeeId = '" + employeeId + "'");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            ButttonText.GetComponent<TextMeshProUGUI>().text = "Dissmiss";

            acceptContract.code -= code;
            acceptContract.art -= art;
            acceptContract.design -= design;

            acceptContract.ShowSelectedContract();
        }
        else
        {
            string query = string.Format("UPDATE employees SET busy='0' WHERE employeeId = '" + employeeId + "'");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            ButttonText.GetComponent<TextMeshProUGUI>().text = "Assign";

            acceptContract.code += code;
            acceptContract.art += art;
            acceptContract.design += design;

            acceptContract.ShowSelectedContract();
        }

    }
}