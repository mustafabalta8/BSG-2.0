using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;

public class AssignEmp : MonoBehaviour
{
    [SerializeField] GameObject AssignEmpObj;
    [SerializeField] GameObject AssignEmpPanel;
    [SerializeField] GameObject SelectedContractObj;

    public List<int> AssignedEmployees = new List<int>();


    DbManager dbManager;
    MoneyManager moneyManager;
    TimeManager timeManager;
    CreateContract createContract;

    public string platform;
    public string sofType;
    public string companyName;
    public int duration;
    public int offer;
    public int code;
    public int art;
    public int design;
    public int ContID;
    public int ProdID;

    [SerializeField] Sprite employee_woman1, employee_woman3, employee_woman2, employee_woman4, employee_woman5, employee_woman6, employee_woman7, employee_woman8, employee_woman9, employee_man1, employee_man2, employee_man3, employee_man4, employee_man5, employee_man6, employee_man7, employee_man8, employee_man9;


    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        timeManager = FindObjectOfType<TimeManager>();
        createContract = FindObjectOfType<CreateContract>();

        ShowEmployeesInAssignPanel();
    }
    public void ClearAssignedEmployessList()
    {
        //AssignedEmployees.Clear();
    }

    public void StartProduction()
    {
        //Debug.Log("Bla: " + code + "  " + design + " bla bla" + "" + art);
        
        //ProcessSld.maxValue = duration;
        Debug.Log("Conract/Product Accepted");
        for (int i = 0; i < AssignedEmployees.Count; i++)
        {
            string query = $"INSERT INTO employees_asign_to_tasks VALUES ({AssignedEmployees[i]},{ProdID},{0})";

            dbManager.ReadRecords(query);
        }
        /* string query02 = $"INSERT INTO taken_contracts VALUES ({ContID},\"{platform}\",\"{sofType}\",{timeManager.displayTime},{duration},{offer},{code},{design},{art})";
         dbManager.ReadRecords(query02);
         */

        dbManager.CloseConnection();
        StartCoroutine(EndProduction(ProdID, AssignedEmployees));


    }
    IEnumerator EndProduction(int ContID, List<int> EmpId)
    {
        Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(duration * timeManager.secondsPerTurn);
        // dismiss employees  / earn money / 
        // moneyManager.changeMoney(offer, "Contract/Product");

        string query = string.Format("DELETE FROM employees_asign_to_tasks  WHERE prod_id ='{0}'", ContID);
        dbManager.DeleteRecords(query);
        for (int i = 0; i < EmpId.Count; i++)
        {
            string query02 = string.Format("UPDATE employees SET busy ='0' WHERE employeeId='{0}'", EmpId[i]);
            dbManager.InsertRecords(query02);
        }

        dbManager.CloseConnection();
        Debug.Log("Contract Deleted/Finished");

    }
    public void ShowEmployeesInAssignPanel()
    {

        string query = "SELECT * FROM employees WHERE hired = 1 AND busy=0";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int employeeId = reader.GetInt32(0);
            string employeeName = reader.GetString(1);

            int code = reader.GetInt32(4);
            int art = reader.GetInt32(5);
            int design = reader.GetInt32(6);

            string profile_pic = reader.GetString(11);

            //GameObject createdEmployee = Instantiate(AssignEmpObj);
            //createdEmployee.transform.SetParent(AssignEmpPanel.transform);
            GameObject createdEmployee = Instantiate(AssignEmpObj,AssignEmpPanel.transform,false);

            Employee EmployeeObj = createdEmployee.GetComponent<Employee>();

            EmployeeObj.employeeId = employeeId;
            EmployeeObj.employeeName = employeeName;
            EmployeeObj.code = code;
            EmployeeObj.art = art;
            EmployeeObj.design = design;

            changeProfilePicture(EmployeeObj.transform, profile_pic);

            EmployeeObj.name = employeeName.ToString();

            EmployeeObj.transform.Find("empName").GetComponent<TextMeshProUGUI>().text = "Name :" + employeeName;
            EmployeeObj.transform.Find("CodeSkill").GetComponent<TextMeshProUGUI>().text = "Coding :" + code.ToString();
            EmployeeObj.transform.Find("ArtSkill").GetComponent<TextMeshProUGUI>().text = "Art :" + art.ToString();
            EmployeeObj.transform.Find("DesignSkill").GetComponent<TextMeshProUGUI>().text = "Design :" + design.ToString();
        }

        dbManager.CloseConnection();

    }
    public void ShowSelectedProduct()
    {
        Contract contractObj = SelectedContractObj.GetComponent<Contract>();

        contractObj.platform = platform;
        contractObj.sofType = sofType;
        contractObj.duration = duration;

        contractObj.offer = offer;
        contractObj.code = code;
        contractObj.art = art;
        contractObj.design = design;
        if (contractObj.code <= 0)
        {
            contractObj.transform.Find("workforce/code").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        if (contractObj.art <= 0)
        {
            contractObj.transform.Find("workforce/art").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        if (contractObj.design <= 0)
        {
            contractObj.transform.Find("workforce/design").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        createContract.CreateContractUI(contractObj);
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
