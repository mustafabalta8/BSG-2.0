using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    Company company;
    DbManager dbManager;
    CreateContract createContract;

    [SerializeField] GameObject AssignEmpObj;
    [SerializeField] GameObject AssignEmpPanel;

    [SerializeField] GameObject SelectedContractObj;
   // [SerializeField] GameObject SelectedContractPanel;

    // Start is called before the first frame update
    void Start()
    {
        company = FindObjectOfType<Company>();
        dbManager = FindObjectOfType<DbManager>();
        createContract = FindObjectOfType<CreateContract>();
        ShowEmployeesInAssignPanel();

   
        

    }
    public void ShowEmployeesInAssignPanel()
    {

            string query = "SELECT * FROM employees WHERE hired = 1";
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
            
            EmployeeObj.transform.Find("empName").GetComponent<TextMeshProUGUI>().text = employeeName;
            EmployeeObj.transform.Find("empName").GetComponent<TextMeshProUGUI>().text = employeeName;
            EmployeeObj.transform.Find("Coding skill").GetComponent<TextMeshProUGUI>().text = code.ToString();
            EmployeeObj.transform.Find("Art skill").GetComponent<TextMeshProUGUI>().text = art.ToString();
            EmployeeObj.transform.Find("Design skill").GetComponent<TextMeshProUGUI>().text = design.ToString();
        }

            dbManager.CloseConnection();
        
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

        createContract.CreateContractUI(contractObj);
    }
    
}
