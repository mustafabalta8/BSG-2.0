using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using TMPro;

public class CreateEmployee : MonoBehaviour
{
    [SerializeField] int powerRangeLimit = 40;
    [SerializeField] double salarayRangeLimit = 5000;

    [SerializeField] GameObject ApplicantsHolder;
    [SerializeField] GameObject OurEmployees;

    //UI
    [SerializeField] TextMeshProUGUI EmpolyeeName;
    [SerializeField] TextMeshProUGUI Code;
    [SerializeField] TextMeshProUGUI Art;
    [SerializeField] TextMeshProUGUI Design;
    [SerializeField] TextMeshProUGUI Salary;

    DbManager dbManager;

    public GameObject ApplicantEmployee;
    private void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
    }

    public Employee createEmployee(int Id, string Name, int salary, int power, int code, int art, int design, bool applicant = false)
    {
        GameObject createdEmployee = Instantiate(ApplicantEmployee);

        if (applicant)
        {
            createdEmployee.transform.SetParent(ApplicantsHolder.transform);
        }

        Employee EmployeeObj = createdEmployee.GetComponent<Employee>();

        EmployeeObj.employeeId = Id;
        EmployeeObj.employeeName = Name;
        EmployeeObj.employeePower = power;
        EmployeeObj.employeeSalary = salary;
        EmployeeObj.code = code;
        EmployeeObj.art = art;
        EmployeeObj.design = design;


        EmployeeObj.name = Name.ToString();

        EmployeeObj.transform.Find("EmployeeName").GetComponent<TextMeshProUGUI>().text = Name.ToString();

        EmployeeObj.transform.Find("CodeSkill").GetComponent<TextMeshProUGUI>().text = "Code: "+code.ToString();
        EmployeeObj.transform.Find("ArtSkill").GetComponent<TextMeshProUGUI>().text = "Art: "+art.ToString();
        EmployeeObj.transform.Find("DesignSkill").GetComponent<TextMeshProUGUI>().text = "Design: "+design.ToString();

        EmployeeObj.transform.Find("SALARY").GetComponent<TextMeshProUGUI>().text = salary.ToString()+"$";

        return EmployeeObj;
    }

    public void createRandomEmployee(int amount)
    {
            string query = "SELECT * FROM employees WHERE hired = 0 ORDER BY random() LIMIT "+amount;
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

                Employee EmployeeObj = createEmployee(employeeId, employeeName, employeeSalary, employeePower, code, art, design, true);

            }
            dbManager.CloseConnection();
    }

    public void createFromDatabase(int employeeId)
    {

    }
}
