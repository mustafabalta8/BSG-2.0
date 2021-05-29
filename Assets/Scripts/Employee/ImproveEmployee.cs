using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImproveEmployee : MonoBehaviour
{
    DbManager dbManager;
    TimeManager timeManager;
    MoneyManager moneyManager;


    [SerializeField] GameObject EmployeesUI;
    [SerializeField] GameObject EmployeesObject;
    // Start is called before the first frame update

    [SerializeField] Sprite green, green_half, red;


    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();

        getEmployees();
    }

    public void getEmployees()
    {
        clearList();

        string query = string.Format("SELECT employeeId FROM employees WHERE hired = 1");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            GameObject employeeObject = Instantiate(EmployeesObject);
            employeeObject.transform.SetParent(EmployeesUI.transform);

            Employee employee = employeeObject.GetComponent<Employee>();

            employee.getDetails(reader.GetInt32(0));

            employee.transform.Find("nameText").GetComponent<TextMeshProUGUI>().text = employee.employeeName;
            employee.transform.Find("positionText").GetComponent<TextMeshProUGUI>().text = employee.position;
            employee.transform.Find("potentialText").GetComponent<TextMeshProUGUI>().text = calculatePotential(employee.age).ToString();

            employee.transform.Find("codeSkillText").GetComponent<TextMeshProUGUI>().text = $"{employee.code/10}/10";
            employee.transform.Find("artSkillText").GetComponent<TextMeshProUGUI>().text = $"{employee.art/10}/10";
            employee.transform.Find("designSkillText").GetComponent<TextMeshProUGUI>().text = $"{employee.design/10}/10";

            employee.transform.Find("codeButton").GetComponentInChildren<TextMeshProUGUI>().text = $"${calculateNewSkillPrice(employee.code, employee.age)}/{calculateNewSkillPrice(employee.code+10, employee.age) / 100}D";
            employee.transform.Find("artButton").GetComponentInChildren<TextMeshProUGUI>().text = $"${calculateNewSkillPrice(employee.art, employee.age)}/{calculateNewSkillPrice(employee.art, employee.age) / 100}D";
            employee.transform.Find("designButton").GetComponentInChildren<TextMeshProUGUI>().text = $"${calculateNewSkillPrice(employee.design, employee.age)}/{calculateNewSkillPrice(employee.design, employee.age) / 100}D";

            if (employee.busy)
            {
                employee.busyObject.SetActive(true);
            }

            employee.transform.Find("moraleText").GetComponent<TextMeshProUGUI>().text = $"{employee.morale}";


            employee.vacationDuration = calculateVacationPrice(employee.morale) / 1000;
            employee.vacationPrice = calculateVacationPrice(employee.morale);


            employee.transform.Find("sendToVacationButton").GetComponentInChildren<TextMeshProUGUI>().text = $"Vacation\n${calculateVacationPrice(employee.morale)}/{calculateVacationPrice(employee.morale)/1000}D";

            int codeSkill, artSkill, designSkill;

            codeSkill = employee.code/10; artSkill = employee.art/10; designSkill = employee.design/10;

            paintSprite("code", codeSkill, employee.transform);
            paintSprite("art", artSkill, employee.transform);
            paintSprite("design", designSkill, employee.transform);
            //break;
        }

        //RectTransform panelRect = (RectTransform)EmployeesUI.transform;
        //Debug.Log("Width: " + panelRect.rect.height);

        dbManager.CloseConnection();
    }

    int calculatePotential(int age)
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

    int calculateNewSkillPrice(int skill, int potential)
    {
        int price;

        if (skill <= 10){
            price = 100;
        }else if (skill >= 11 && skill <= 20){
            price = 200;
        }else if (skill >= 21 && skill <= 30){
            price = 300;
        }else if (skill >= 31 && skill <= 40){
            price = 400;
        }else if (skill >= 41 && skill <= 50){
            price = 500;
        }else if (skill >= 51 && skill <= 60){
            price = 600;
        }else if (skill >= 61 && skill <= 70){
            price = 700;
        }else if (skill >= 71 && skill <= 80){
            price = 800;
        }else if (skill >= 81 && skill <= 90){
            price = 900;
        }else{
            price = 1000;
        }


        if (potential == 90)
        {
            price += 10;
        }
        else if (potential == 70)
        {
            price += 50;
        }
        else if (potential == 50)
        {
            price += 70;
        }
        else if (potential == 40)
        {
            price += 90;
        }
        else
        {
            price += 150;
        }
        return price;
    }

    int calculateVacationPrice(int morale)
    {
        int price;

        if (morale <= 10){
            price = 5000;
        }else if (morale >= 11 && morale <= 20){
            price = 4500;
        }else if (morale >= 21 && morale <= 30){
            price = 4000;
        }else if (morale >= 31 && morale <= 40){
            price = 3500;
        }else if (morale >= 41 && morale <= 50){
            price = 3000;
        }else if (morale >= 51 && morale <= 60){
            price = 3200;
        }else if (morale >= 61 && morale <= 70){
            price = 3100;
        }else if (morale >= 71 && morale <= 80){
            price = 3000;
        }else if (morale >= 81 && morale <= 90){
            price = 2800;
        }else{
            price = 2500;
        }
        return price;
    }

    public void changeSprite(Transform gameObject, string skillType, int x, Sprite sprite)
    {
        gameObject.Search($"{skillType}_{x}").GetComponent<Image>().sprite = sprite;
    }

    public void paintSprite(string skillType, int skill, Transform gameObject)
    {
        if (skill == 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                //Debug.Log($"All red, skill: {skill}");
                changeSprite(gameObject, skillType, i, red);
            }
        }
        else
        {
            for (int i = 1; i <= 5; i++)
            {
                if (skill == 0) break;
                if (skill < 2)
                {
                    //Debug.Log($"One green half, skill: {skill}");
                    changeSprite(gameObject, skillType, i, green_half);
                    skill -= 1;
                }
                else if (skill >= 2)
                {
                    //Debug.Log($"One green, skill: {skill}");
                    changeSprite(gameObject, skillType, i, green);
                    skill -= 2;
                }
                else
                {
                    //Debug.Log($"One red, skill: {skill}");
                    changeSprite(gameObject, skillType, i, red);
                    skill -= 0;
                }
            }
        }
    }

    public void improveEmploye(Employee employee, string skillType)
    {
        if(skillType == "code")
        {
            employee.code += 10;
            employee.skillUpgragePrice = calculateNewSkillPrice(employee.code, employee.age);
            employee.skillDuration = employee.skillUpgragePrice / 100;
        }
        else if(skillType == "art")
        {
            employee.art += 10;
            employee.skillUpgragePrice = calculateNewSkillPrice(employee.art, employee.age);
            employee.skillDuration = employee.skillUpgragePrice / 100;
        }
        else
        {
            employee.design += 10;
            employee.skillUpgragePrice = calculateNewSkillPrice(employee.design, employee.age);
            employee.skillDuration = employee.skillUpgragePrice / 100;
        }

        fixButtons(employee, skillType);
    }

    public void fixButtons(Employee employee, string skillType)
    {
        Button codeButton = employee.codeButton;
        Button artButton = employee.artButton;
        Button designButton = employee.designButton;
        Button trainButton = employee.trainButton;

        if (skillType == "code")
        {
            if (!artButton.interactable)
            {
                artButton.interactable = true;
                designButton.interactable = true;
                trainButton.interactable = false;
                codeButton.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                artButton.interactable = false;
                designButton.interactable = false;
                trainButton.interactable = true;
                codeButton.GetComponent<Image>().color = Color.green;
            }
        }
        else if (skillType == "art")
        {
            if (!codeButton.interactable)
            {
                codeButton.interactable = true;
                designButton.interactable = true;
                trainButton.interactable = false;
                artButton.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                codeButton.interactable = false;
                designButton.interactable = false;
                trainButton.interactable = true;
                artButton.GetComponent<Image>().color = Color.green;
            }
        }
        else
        {
            if (!artButton.interactable)
            {
                codeButton.interactable = true;
                artButton.interactable = true;
                trainButton.interactable = false;
                designButton.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                codeButton.interactable = false;
                artButton.interactable = false;
                trainButton.interactable = true;
                designButton.GetComponent<Image>().color = Color.green;
            }
        }
    }

    public void clearList(string employeeName = null)
    {
        foreach (Transform company in EmployeesUI.transform)
        {
            if (company.name == "List title") continue;
            else
            {
                if (employeeName != null)
                {
                    if (company.name == employeeName)
                    {
                        GameObject.Destroy(company.gameObject);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    GameObject.Destroy(company.gameObject);

                }
            }
        }
    }

}
