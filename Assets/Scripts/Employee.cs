using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
using System.Collections;

public class Employee : MonoBehaviour
{
    public int employeeId, employeeSalary, employeePower, code, art, design, morale, age;
    public string employeeName, position;
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
    public void Start()
    {
        acceptContract = FindObjectOfType<AcceptContract>();
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();
        dbManager = FindObjectOfType<DbManager>();
        timeManager = FindObjectOfType<TimeManager>();
        improve = FindObjectOfType<ImproveEmployee>();
    }

    public void getDetails(int employeeId)
    {
        dbManager = FindObjectOfType<DbManager>();
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

            if (reader.GetInt32(8) == 1)
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

            morale = reader.GetInt32(9);
        }

        dbManager.CloseConnection();
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
            string query = string.Format("UPDATE employees SET busy='1' WHERE employeeId = '" + employeeId + "' ");

            dbManager.InsertRecords(query);
            dbManager.CloseConnection();
            ButttonText.GetComponent<TextMeshProUGUI>().text = "Dissmiss";

            acceptContract.AssignedEmployees.Add(employeeId);
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

            acceptContract.AssignedEmployees.Remove(employeeId);
            acceptContract.code += code;
            acceptContract.art += art;
            acceptContract.design += design;

            acceptContract.ShowSelectedContract();
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

        Debug.Log("Training finished");
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

        query = string.Format($"UPDATE employees SET morale={morale + 10} WHERE employeeName = \"{employeeName}\"");
        dbManager.InsertRecords(query);
        dbManager.CloseConnection();

        morale += 10;

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

        Debug.Log($"Vacation finished, new morale: {morale + 10}");
    }

}