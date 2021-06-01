using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
public class AcceptContract : MonoBehaviour
{
    public string platform;
    public string sofType;
    public string companyName;
    public int duration;
    public int offer;
    public int code;
    public int art;
    public int design;
    public int ContID;

    Company company;
    DbManager dbManager;
    CreateContract createContract;
    MoneyManager moneyManager;
    TimeManager timeManager;

    [SerializeField] GameObject AssignEmpObj;
    [SerializeField] GameObject AssignEmpPanel;

    [SerializeField] GameObject SelectedContractObj;

    public List<int> AssignedEmployees = new List<int>();
    [SerializeField] Slider ProcessSld;
    
    // [SerializeField] GameObject SelectedContractPanel;

    // Start is called before the first frame update
    void Start()
    {
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();
        createContract = FindObjectOfType<CreateContract>();
        moneyManager = FindObjectOfType<MoneyManager>();
        timeManager = FindObjectOfType<TimeManager>();
        //ShowEmployeesInAssignPanel();

    }
    public void TakeContract()
    {
        //ProcessSld.maxValue = duration;
        Debug.Log("Contract Accepted");
        for (int i=0;i< AssignedEmployees.Count; i++)
        {
        string query = $"INSERT INTO employees_asign_to_tasks VALUES ({AssignedEmployees[i]},{0},{ContID})";

            dbManager.ReadRecords(query);
        }
        string query02 = $"INSERT INTO taken_contracts VALUES ({ContID},\"{platform}\",\"{sofType}\",{timeManager.displayTime},{duration},{offer},{code},{design},{art})";
        dbManager.ReadRecords(query02);
        dbManager.CloseConnection();

        createContract.DestroyContract(ContID);//Can't remove RectTransform because Image (Script), Image (Script), VerticalLayoutGroup (Script), VerticalLayoutGroup (Script), VerticalLayoutGroup (Script) depends on it

        StartCoroutine(EndProduction(ContID, AssignedEmployees));





    }
    IEnumerator EndProduction(int ContID,List<int> EmpId)
    {
        Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(duration* timeManager.secondsPerTurn);
        // dismiss employees  / earn money / 
        moneyManager.changeMoney(offer,"Contract");

        string query = string.Format("DELETE FROM employees_asign_to_tasks  WHERE con_id ='{0}'", ContID);
        dbManager.DeleteRecords(query);
        for(int i =0; i < EmpId.Count; i++)
        {
            string query02 = string.Format("UPDATE employees SET busy ='0' WHERE employeeId='{0}'", EmpId[i]);
            dbManager.InsertRecords(query02);
        }
        
        dbManager.CloseConnection();
        Debug.Log("Contract Deleted/Finished");

    }
    public void ShowSelectedContract()
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
    /*
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


                GameObject createdEmployee = Instantiate(AssignEmpObj);
                createdEmployee.transform.SetParent(AssignEmpPanel.transform);

                Employee EmployeeObj = createdEmployee.GetComponent<Employee>();
 
            EmployeeObj.employeeId = employeeId;
            EmployeeObj.employeeName = employeeName;
            EmployeeObj.code = code;
            EmployeeObj.art = art;
            EmployeeObj.design = design;



            EmployeeObj.name = employeeName.ToString();             
            
            EmployeeObj.transform.Find("empName").GetComponent<TextMeshProUGUI>().text = "Name :"+employeeName;
            EmployeeObj.transform.Find("CodeSkill").GetComponent<TextMeshProUGUI>().text = "Coding :" + code.ToString();
            EmployeeObj.transform.Find("ArtSkill").GetComponent<TextMeshProUGUI>().text = "Art :" + art.ToString();
            EmployeeObj.transform.Find("DesignSkill").GetComponent<TextMeshProUGUI>().text = "Design :" + design.ToString();
        }

            dbManager.CloseConnection();
        
    }*/





}
