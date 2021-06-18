using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
using System.Collections;

public class Employee : MonoBehaviour
{
    public int employeeId, employeeSalary, employeePower, code, art, design, morale, age;
    public string employeeName, position, profile_pic;
    public bool busy;

    public int skillDuration, skillUpgragePrice, vacationDuration, vacationPrice;

    [SerializeField] Button Hire;  // connected with the HireButton via prefab
    [SerializeField] TextMeshProUGUI ButttonText;

    [SerializeField] Image code_1, code_2, code_3, code_4, code_5;
    [SerializeField] Image art_1, art_2, art_3, art_4, art_5;
    [SerializeField] Image design_1, design_2, design_3, design_4, design_5;

    [SerializeField] public Button codeButton, artButton, designButton, trainButton, vacationButton;

    [SerializeField] public GameObject busyObject, underTrainingObject, onVacationObject;

    public string selectedSkill = "";

    Company company;
    DbManager dbManager;
    AcceptContract acceptContract;
    ImproveEmployee improve;
    TimeManager timeManager;
    AssignEmp assignEmp;
    Notifications notifications;

    public void Start()
    {
        assignEmp = FindObjectOfType<AssignEmp>();
        acceptContract = FindObjectOfType<AcceptContract>();
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();
        dbManager = FindObjectOfType<DbManager>();
        timeManager = FindObjectOfType<TimeManager>();
        improve = FindObjectOfType<ImproveEmployee>();
        notifications = FindObjectOfType<Notifications>();

    }

    public void getDetails(int employeeId)
    {
        dbManager = FindObjectOfType<DbManager>();

        company = FindObjectOfType<Company>();

        string query = string.Format($"SELECT * FROM employees WHERE employeeId = {employeeId}");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            this.employeeId = employeeId;
            employeeName = reader.GetString(1);
            employeeSalary = reader.GetInt32(2);
            employeePower = reader.GetInt32(3);

            code = reader.GetInt32(4);
            art = reader.GetInt32(5);
            design = reader.GetInt32(6);

            age = reader.GetInt32(7);

            if (reader.GetInt32(9) == 1)
            {
                busy = true;
            }
            else
            {
                busy = false;
            }

            if (code > art)
            {
                if (code > design)
                {
                    position = "Developer";
                }
                else
                {
                    position = "Designer";
                }
            }
            else if (art > design)
                position = "Artist";
            else
                position = "Designer";

            morale = reader.GetInt32(10);

            if (morale > 100)
            {
                morale = 100;
            }

            profile_pic = reader.GetString(11);
        }

        dbManager.CloseConnection();

        if (company.instructive_leader)
        {
            switch (company.instructive_leader_level)
            {
                case 1:
                    code = (int)(code * (1 + 0.05));
                    art = (int)(art * (1 + 0.05));
                    design = (int)(design * (1 + 0.05));
                    break;
                case 2:
                    code = (int)(code * (1 + 0.10));
                    art = (int)(art * (1 + 0.10));
                    design = (int)(design * (1 + 0.10));
                    break;
            }
        }
    }

    public void hireEmployee()
    {

        if (company.dream_team)
        {
            switch (company.dream_team_level)
            {
                case 1:
                    if(company.officeId == 0)
                    {
                        company.OurOfficeCapacity = 3;
                    }
                    else
                    {
                        company.OurOfficeCapacity = 12;
                    }
                    break;
                case 2:
                    if (company.officeId == 0)
                    {
                        company.OurOfficeCapacity = 4;
                    }
                    else
                    {
                        company.OurOfficeCapacity = 13;
                    }
                    break;
                case 3:
                    if (company.officeId == 0)
                    {
                        company.OurOfficeCapacity = 4;
                    }
                    else
                    {
                        company.OurOfficeCapacity = 15;
                    }
                    break;
            }
        }

        if (company.OurOfficeCapacity > company.employees.Count)
        {
            company.AddEmployeeToCompany(this);
            company.EmployeeAnimation();
            Destroy(gameObject);
        }
        else
        {
            notifications.pushNotification($"You do not have enough capacity to hire new employees. Your current capacity is {company.OurOfficeCapacity}");
        }         
    }

    public void fireEmployee()
    {
        company.FireEmployee(this);
        company.EmployeeAnimation();
        Destroy(gameObject);
    }
    public void assignEmployee()
    {
        if (ButttonText.GetComponent<TextMeshProUGUI>().text == "Assign")
        {
            string query = string.Format("UPDATE employees SET busy='1' WHERE employeeId = '" + employeeId + "' ");

            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            ButttonText.GetComponent<TextMeshProUGUI>().text = "Dissmiss";

            acceptContract.AssignedEmployees.Add(employeeId);
            acceptContract.code -= code;
            acceptContract.art -= art;
            acceptContract.design -= design;

            acceptContract.ShowSelectedContract();

            assignEmp.AssignedEmployees.Add(employeeId);
            assignEmp.code -= code;
            assignEmp.art -= art;
            assignEmp.design -= design;
            assignEmp.ShowSelectedProduct();


        }
        else
        {
            string query = string.Format("UPDATE employees SET busy='0' WHERE employeeId = '" + employeeId + "'");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            ButttonText.GetComponent<TextMeshProUGUI>().text = "Assign";

            acceptContract.AssignedEmployees.Remove(employeeId);
            acceptContract.code += code;
            acceptContract.art += art;
            acceptContract.design += design;

            acceptContract.ShowSelectedContract();

            assignEmp.AssignedEmployees.Remove(employeeId);
            assignEmp.code += code;
            assignEmp.art += art;
            assignEmp.design += design;

            assignEmp.ShowSelectedProduct();
        }

    }

    public void selectSkill(string skillName)
    {
        selectedSkill = skillName;
        improve.improveEmploye(this, skillName);
    }

    public void improveSkills()
    {
        StartCoroutine(saveSkills());
    }

    IEnumerator saveSkills()
    {
        bool busy = false;

        Debug.Log($"Training started, will wait for {(skillDuration - 1) * timeManager.secondsPerTurn} seconds (duration: {(skillDuration - 1)})");

        codeButton.interactable = false;
        artButton.interactable = false;
        designButton.interactable = false;
        trainButton.interactable = false;

        string query = string.Format($"SELECT busy FROM employees WHERE employeeName = \"{employeeName}\"");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int busy_data = reader.GetInt32(0);

            busy = false;
            if (busy_data == 1)
            {
                busy = true;
                Debug.Log($"Employee was busy, busy set to false");
            }

        }

        dbManager.CloseConnection();


        if (busy)
        {
            query = string.Format($"UPDATE employees SET busy=0 WHERE employeeName = \"{employeeName}\"");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
        }

        underTrainingObject.SetActive(true);

        yield return new WaitForSeconds((skillDuration - 1) * timeManager.secondsPerTurn);

        query = string.Format($"UPDATE employees SET code='{code}', art={art}, design={design} WHERE employeeName = \"{employeeName}\"");
        dbManager.InsertRecords(query);
        dbManager.CloseConnection();

        improve.getEmployees();

        codeButton.interactable = true;
        artButton.interactable = true;
        designButton.interactable = true;
        trainButton.interactable = true;

        codeButton.GetComponent<Image>().color = Color.yellow;
        artButton.GetComponent<Image>().color = Color.yellow;
        designButton.GetComponent<Image>().color = Color.yellow;

        if (busy)
        {
            query = string.Format($"UPDATE employees SET busy=1 WHERE employeeName = \"{employeeName}\"");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            Debug.Log($"Employee was busy, busy set to true");
        }

        underTrainingObject.SetActive(false);

        improve.getEmployees();

        notifications.pushNotification($"Training finished for {employeeName}, employee is back to work.");
    }

    public void improveMorale()
    {
        StartCoroutine(saveMorale());
    }

    IEnumerator saveMorale()
    {
        Debug.Log($"Vacation started, will last for {(vacationDuration - 1) * timeManager.secondsPerTurn}");

        string query = string.Format($"SELECT busy FROM employees WHERE employeeName = \"{employeeName}\"");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int busy_data = reader.GetInt32(0);

            busy = false;
            if (busy_data == 1)
            {
                busy = true;
            }

        }
        dbManager.CloseConnection();


        if (busy)
        {
            query = string.Format($"UPDATE employees SET busy=0 WHERE employeeName = \"{employeeName}\"");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            Debug.Log($"Employee was busy, busy set to false");
        }


        onVacationObject.SetActive(true);
        vacationButton.interactable = false;
        trainButton.interactable = false;

        codeButton.interactable = false;
        artButton.interactable = false;
        designButton.interactable = false;

        yield return new WaitForSeconds((vacationDuration - 1) * timeManager.secondsPerTurn);

        int newMorale = morale + 10;

        if (company.business_class)
        {
            switch (company.business_class_level)
            {
                case 1:
                    newMorale = (int)(newMorale * (1 + 0.25));
                    break;
                case 2:
                    newMorale = (int)(newMorale * (1 + 0.25));
                    break;
            }
        }

        query = string.Format($"UPDATE employees SET morale={newMorale} WHERE employeeName = \"{employeeName}\"");
        dbManager.InsertRecords(query);
        dbManager.CloseConnection();

        morale = newMorale;

        if (busy)
        {
            query = string.Format($"UPDATE employees SET busy=1 WHERE employeeName = \"{employeeName}\"");
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            Debug.Log($"Employee was busy, busy set to true");
        }

        onVacationObject.SetActive(false);
        vacationButton.interactable = true;

        codeButton.interactable = true;
        artButton.interactable = true;
        designButton.interactable = true;

        improve.getEmployees();

        notifications.pushNotification($"{employeeName} is back from vacation. Employee is back to work.");
    }

    public int calculatePotential()
    {
        int potential;

        if (age <= 20)
        {
            potential = 100;
        }
        else if (age >= 21 && age <= 25)
        {
            potential = 90;
        }
        else if (age >= 26 && age <= 30)
        {
            potential = 70;
        }
        else if (age >= 31 && age <= 40)
        {
            potential = 50;
        }
        else if (age >= 41 && age <= 50)
        {
            potential = 40;
        }
        else
        {
            potential = 30;
        }

        return potential;
    }

}