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

    [SerializeField] Sprite employee_woman1, employee_woman3, employee_woman2, employee_woman4, employee_woman5, employee_woman6, employee_woman7, employee_woman8, employee_woman9, employee_man1, employee_man2, employee_man3, employee_man4, employee_man5, employee_man6, employee_man7, employee_man8, employee_man9;

    public GameObject ApplicantEmployee;
    private void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
    }

    public Employee createEmployee(int Id, string Name, int salary, int power, int code, int art, int design, string profile_pic, bool applicant = false)
    {
       /* GameObject createdEmployee = Instantiate(ApplicantEmployee);

        if (applicant)
        {
            createdEmployee.transform.SetParent(ApplicantsHolder.transform);
        }*/

        GameObject createdEmployee = Instantiate(ApplicantEmployee,ApplicantsHolder.transform,false);

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

        changeProfilePicture(EmployeeObj.transform, profile_pic);

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
                string profile_pic = reader.GetString(11);

                Employee EmployeeObj = createEmployee(employeeId, employeeName, employeeSalary, employeePower, code, art, design, profile_pic, true);

            }
            dbManager.CloseConnection();
    }


    public void changeProfilePicture(Transform gameObject, string employee_pic)
    {
        Sprite sprite;
        if(employee_pic == "employee_woman1"){
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
