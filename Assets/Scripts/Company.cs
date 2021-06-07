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
    [SerializeField] List<Emp_Place_Points> emp_Place_Points;
    List<Transform> Waypoints;
    [SerializeField] GameObject EmployeeAnm;//calisan1, calisan2, ...
    [SerializeField] GameObject EmployeeAnmHolder;

    [SerializeField] GameObject MyEmployeeHolder;
    [SerializeField] GameObject MyEmployee;
    [SerializeField] Sprite employee_woman1, employee_woman3, employee_woman2, employee_woman4, employee_woman5, employee_woman6, employee_woman7, employee_woman8, employee_woman9, employee_man1, employee_man2, employee_man3, employee_man4, employee_man5, employee_man6, employee_man7, employee_man8, employee_man9;

    [Header("Bank")]
    public int balance;

    void Start()
    {
        Waypoints = emp_Place_Points[0].GetPoints();

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
            OurOffice.transform.Find("capacity").GetComponent<TextMeshProUGUI>().text = "" + reader.GetInt32(1);
            OurOffice.transform.Find("rent").GetComponent<TextMeshProUGUI>().text = "" + reader.GetInt32(2);
            OurOffice.transform.Find("furniture").GetComponent<TextMeshProUGUI>().text = "" + (reader.GetInt32(3)+ reader.GetInt32(4)+reader.GetInt32(5));
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
        int i = 0;
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

            changeProfilePicture(EmployeeObj.transform, EmployeeObj.profile_pic);


            //CalisanMasasi1
            GameObject EmpAnm = Instantiate(EmployeeAnm, Waypoints[i].position, Quaternion.identity);
            EmpAnm.transform.SetParent(EmployeeAnmHolder.transform);
            
            /*
            if (EmployeeObj.profile_pic == calisan1(EmployeeAnm))
            {
                createEmpAnimation(calisan1, i);
            }else if(EmployeeObj.profile_pic == calisan2(EmployeeAnm))
            {
                createEmpAnimation(calisan2, i);
            }
            */

            i++;

        }

        dbManager.CloseConnection();
    }
    void createEmpAnimation(GameObject EmployeeAnm, int i)
    {
        GameObject EmpAnm = Instantiate(EmployeeAnm, Waypoints[i].position, Quaternion.identity);
        EmpAnm.transform.SetParent(EmployeeAnmHolder.transform);
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

    public void changeProfilePicture(Transform gameObject, string employee_pic)
    {
        Sprite sprite;
        if (employee_pic == "employee_woman1")
        {
            sprite = employee_woman1;
        }
        else if (employee_pic == "employee_woman2")
        {
            sprite = employee_woman2;
        }
        else if (employee_pic == "employee_woman3")
        {
            sprite = employee_woman3;
        }
        else if (employee_pic == "employee_woman4")
        {
            sprite = employee_woman4;
        }
        else if (employee_pic == "employee_woman5")
        {
            sprite = employee_woman5;
        }
        else if (employee_pic == "employee_woman6")
        {
            sprite = employee_woman6;
        }
        else if (employee_pic == "employee_woman7")
        {
            sprite = employee_woman7;
        }
        else if (employee_pic == "employee_woman8")
        {
            sprite = employee_woman8;
        }
        else if (employee_pic == "employee_woman9")
        {
            sprite = employee_woman9;
        }
        else if (employee_pic == "employee_man1")
        {
            sprite = employee_man1;
        }
        else if (employee_pic == "employee_man2")
        {
            sprite = employee_man2;
        }
        else if (employee_pic == "employee_man3")
        {
            sprite = employee_man3;
        }
        else if (employee_pic == "employee_man4")
        {
            sprite = employee_man4;
        }
        else if (employee_pic == "employee_man5")
        {
            sprite = employee_man5;
        }
        else if (employee_pic == "employee_man6")
        {
            sprite = employee_man6;
        }
        else if (employee_pic == "employee_man7")
        {
            sprite = employee_man7;
        }
        else if (employee_pic == "employee_man8")
        {
            sprite = employee_man8;
        }
        else
        {
            sprite = employee_man9;
        }

        gameObject.Search("Employee Photo").GetComponent<Image>().sprite = sprite;

    }

}
