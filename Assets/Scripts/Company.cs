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
            GameObject createdEmployee = Instantiate(MyEmployee);
            createdEmployee.transform.SetParent(MyEmployeeHolder.transform);

            Employee EmployeeObj = createdEmployee.GetComponent<Employee>();

            int employeeId = reader.GetInt32(0);

            EmployeeObj.getDetails(employeeId);


            this.companyPower = companyPower + EmployeeObj.employeePower;

            employees.Add(EmployeeObj);

            totalSalary = totalSalary + EmployeeObj.employeeSalary;


            EmployeeObj.transform.Find("empName").GetComponent<TextMeshProUGUI>().text = EmployeeObj.employeeName;
            EmployeeObj.transform.Find("Employee position").GetComponent<TextMeshProUGUI>().text = EmployeeObj.position;
            EmployeeObj.transform.Find("Employee potential").GetComponent<TextMeshProUGUI>().text = $"{EmployeeObj.calculatePotential()}%";
            EmployeeObj.transform.Find("Employee Salary").GetComponent<TextMeshProUGUI>().text = $"${EmployeeObj.employeeSalary}";
            if (EmployeeObj.busy)
            {
                EmployeeObj.transform.Find("Employee status").GetComponent<TextMeshProUGUI>().text = "Busy";

            }
            else
            {
                EmployeeObj.transform.Find("Employee status").GetComponent<TextMeshProUGUI>().text = "Available";
            }
            EmployeeObj.transform.Find("Coding skill").GetComponent<TextMeshProUGUI>().text = $"{EmployeeObj.code}";
            EmployeeObj.transform.Find("Art skill").GetComponent<TextMeshProUGUI>().text = $"{EmployeeObj.art}";
            EmployeeObj.transform.Find("Design skill").GetComponent<TextMeshProUGUI>().text = $"{EmployeeObj.design}";
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
