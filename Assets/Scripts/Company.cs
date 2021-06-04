using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using UnityEngine.UI;
using System;
using TMPro;

public class Company : MonoBehaviour
{
    [Header("Office Related")]
    public int OurOfficeCapacity;
    public int OurOfficeRent;
    public int motivation;
    [SerializeField] GameObject OurOffice;

    [Header("General Company Info")]
    public string companyName;
    public int companyPower = 0;
    public int totalSalary = 0;
    public int credibility = 10;
    public List<Employee> employees = new List<Employee>();

    public KeyValuePair<string, int> skill = new KeyValuePair<string, int>();

    DbManager dbManager;
    CreateEmployee employeeFactory;

    [Header("Employee Related")]
    [SerializeField] GameObject MyEmployeeHolder;
    [SerializeField] GameObject MyEmployee;

    [Header("Bank")]
    public int balance;

    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        employeeFactory = FindObjectOfType<CreateEmployee>(); //get the employee script

        getEmployeesFromDatabase(employeeFactory);
        getCompanyDataFromDatabase();

        employeeFactory.createRandomEmployee(5); //create X random employees

        ShowUpdateOnOfficeValues();
    }
    public void ShowUpdateOnOfficeValues()
    {
        string query = "SELECT * FROM office WHERE id = 1";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            OurOffice.transform.Find("capacity").GetComponent<TextMeshProUGUI>().text = "Office Capacity:" + reader.GetInt32(1);
            OurOffice.transform.Find("rent").GetComponent<TextMeshProUGUI>().text = "Rent:" + reader.GetInt32(2);
            OurOffice.transform.Find("furniture").GetComponent<TextMeshProUGUI>().text = "Total Furniture:" + (reader.GetInt32(3)+ reader.GetInt32(4)+reader.GetInt32(5));
        }
        dbManager.CloseConnection();
    }

    void getEmployeesFromDatabase(CreateEmployee employeeFactory) {

        employees.Clear();

        foreach (Transform employee in MyEmployeeHolder.transform)
        {
            GameObject.Destroy(employee.gameObject);
        }


        string query = "SELECT * FROM employees WHERE hired = 1";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int employeeId = reader.GetInt32(0);
            string employeeName = reader.GetString(1);
            int employeeSalary = reader.GetInt32(2);
            int employeePower = reader.GetInt32(3);
            int code = reader.GetInt32(4);
            int art = reader.GetInt32(5);
            int design = reader.GetInt32(6);

            int morale = reader.GetInt32(10);
            string profile_pic = reader.GetString(11);

            this.companyPower = companyPower + employeePower;


            GameObject createdEmployee = Instantiate(MyEmployee);
            createdEmployee.transform.SetParent(MyEmployeeHolder.transform);

            Employee EmployeeObj = createdEmployee.GetComponent<Employee>();

            employees.Add(EmployeeObj);

            EmployeeObj.employeeId = employeeId;
            EmployeeObj.employeeName = employeeName;
            EmployeeObj.employeePower = employeeSalary;
            EmployeeObj.employeeSalary = employeePower;
            EmployeeObj.morale = morale;
            EmployeeObj.profile_pic = profile_pic;

            EmployeeObj.name = employeeName.ToString();

            totalSalary = totalSalary + EmployeeObj.employeeSalary;

        }

        dbManager.CloseConnection();
    }

    public void getCompanyDataFromDatabase()
    {
        dbManager = FindObjectOfType<DbManager>();
        string query = "SELECT * FROM company";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            string companyName = reader.GetString(0);
            int balance = reader.GetInt32(1);
            int credibility = reader.GetInt32(2);

            this.companyName = companyName;
            this.balance = balance;
            this.credibility = credibility;
        }

        dbManager.CloseConnection();
    }

    public void AddEmployeeToCompany(Employee employee)
    {

        if (!employees.Any())
        {
            employees.Add(employee);
            
            string query = string.Format("UPDATE employees SET hired='1' WHERE employeeId = '"+employee.employeeId+"'");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();

            Debug.Log(employee.employeeName + " is hired as first employee");

            getEmployeesFromDatabase(employeeFactory);
            return;
        }
        else
        {
            foreach (var currentEmployee in employees.ToList())
            {
                Debug.Log(currentEmployee.employeeName);

                if (employee.employeeId == currentEmployee.employeeId)
                {
                    Debug.Log(employee.employeeName+" is already hired");
                    return;
                }
                else
                {
                    employees.Add(employee);

                    string query = string.Format("UPDATE employees SET hired='1' WHERE employeeId = '" + employee.employeeId + "'"); dbManager.InsertRecords(query);
                    dbManager.CloseConnection();

                    Debug.Log(employee.employeeName + " is hired");
                    getEmployeesFromDatabase(employeeFactory);
                    return;
                }
            }
        }
    }

    public void FireEmployee(Employee employee)
    {
        string query = string.Format("UPDATE employees SET hired='0' WHERE employeeId = '" + employee.employeeId + "'"); dbManager.InsertRecords(query);
        dbManager.DeleteRecords(query);
        dbManager.CloseConnection();
        getEmployeesFromDatabase(employeeFactory);
    }

    public void saveCompanyData()
    {
        string query = string.Format($"UPDATE company SET companyName = \"{companyName}\", bank = {balance}, credibility = {credibility}"); dbManager.InsertRecords(query);
        dbManager.CloseConnection();
    }
}
