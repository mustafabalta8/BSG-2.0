using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.Linq;

public class Employee : MonoBehaviour
{
    public int employeeId;
    public string employeeName;
    public int employeeSalary;
    public int employeePower;

    public int code, art, design;

    [SerializeField] Button Hire;  // connected with the HireButton via prefab

    Company company;
    DbManager dbManager;

    public void Start()
    {
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();

       // employeeManager.ListEmployees();
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
        
    }
}