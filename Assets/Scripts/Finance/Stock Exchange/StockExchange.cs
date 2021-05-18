using UnityEngine;
using System.Data;
using TMPro;


public class StockExchange : MonoBehaviour
{
    DbManager dbManager;
    TimeManager timeManager;
    MoneyManager moneyManager;
    BoughtShares boughtShares;

    [SerializeField] GameObject SEUi;
    [SerializeField] GameObject SECompanyObj;

    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        boughtShares = FindObjectOfType<BoughtShares>();

        getCompanies();
    }
    public void getCompanies()
    { 
        string query = string.Format("SELECT * FROM all_companies ORDER BY brandValue DESC");//where company=companyName;
        IDataReader reader = dbManager.ReadRecords(query);

        int rank = 1;
        while (reader.Read())
        {
            string companyName = reader.GetString(0);
            int brandValue = reader.GetInt32(1);
            int companyPower = reader.GetInt32(2);
            int budget = reader.GetInt32(3);
            int fans = reader.GetInt32(4);
            int shares = reader.GetInt32(5);
            
            GameObject createdCompany = Instantiate(SEUi);
            createdCompany.transform.SetParent(SECompanyObj.transform);

            SC_Company companyObject = createdCompany.GetComponent<SC_Company>();

            companyObject.companyName = companyName;
            companyObject.brandValue = brandValue;
            companyObject.companyPower = companyPower;
            companyObject.budget = budget;
            companyObject.fans = fans;
            companyObject.shares = shares;

            companyObject.name = companyName.ToString();

            int price = ((budget + (companyPower / brandValue)) / 100);

            companyObject.price = price;

            companyObject.transform.Find("rank").GetComponent<TextMeshProUGUI>().text = $"{rank}";
            companyObject.transform.Find("companyName").GetComponent<TextMeshProUGUI>().text = companyName;
            companyObject.transform.Find("brandValue").GetComponent<TextMeshProUGUI>().text = brandValue.ToString();
            companyObject.transform.Find("price").GetComponent<TextMeshProUGUI>().text = $"${price}";
            companyObject.transform.Find("shares").GetComponent<TextMeshProUGUI>().text = $"{shares}/100";

            rank++;
        }

        dbManager.CloseConnection();
    }

    public void clearList(string companyName = null)
    {
        foreach (Transform company in SECompanyObj.transform)
        {
            if (company.name == "List title") continue;
            else
            {
                if(companyName != null)
                {
                    if(company.name == companyName)
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

    public void buyShares(SC_Company company)
    {
        string query = $"INSERT INTO bought_shares VALUES (\"{company.companyName}\",{timeManager.displayTime},{company.price})";
        dbManager.ReadRecords(query);
        dbManager.CloseConnection();

        if(company.brandValue != 100)
        {
            company.brandValue++;
            company.budget += company.price;
        }
        else
        {
            company.budget += company.price;
        }

        if (company.shares > 0)
        {
            company.shares--;
        }
        else
        {
            clearList(company.name);
        }

        company.updateCompany();

        moneyManager.changeMoney(-(company.price),"Stock exchange");

        clearList();
        getCompanies();

        Debug.Log($"1 {company.companyName} share bought at {company.price}");

        boughtShares.getAllBoughtShares();
    }
}
