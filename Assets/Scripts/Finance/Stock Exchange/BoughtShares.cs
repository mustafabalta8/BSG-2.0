using UnityEngine;
using System.Data;
using TMPro;
using System;

public class BoughtShares : MonoBehaviour
{

    DbManager dbManager;
    TimeManager timeManager;
    MoneyManager moneyManager;
    SC_Company SC_company;

    [SerializeField] GameObject BoughtSharesUI;
    [SerializeField] GameObject BoughtSharesCompanyObject;

    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        SC_company = FindObjectOfType<SC_Company>();

        getAllBoughtShares();
    }

    public void getAllBoughtShares()
    {
        clearList(BoughtSharesCompanyObject);

        string query = string.Format("SELECT company_name, ROUND(AVG(buy_price)) as averageBuyPrice, COUNT(company_name) as nShares FROM bought_shares GROUP BY company_name ORDER BY nShares DESC ");//where company=companyName;
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            string companyName = reader.GetString(0);
            double avgBuyPrice = reader.GetDouble(1);
            double nShares = reader.GetDouble(2);

            GameObject createdCompany = Instantiate(BoughtSharesUI);
            createdCompany.transform.SetParent(BoughtSharesCompanyObject.transform);

            SC_Company companyObject = createdCompany.GetComponent<SC_Company>();
            companyObject.companyName = companyName;

            SC_Company createdBoughtCompany = new SC_Company(companyName);

            int price_now = createdBoughtCompany.getStockPrice();

            double change = Math.Round(((price_now - avgBuyPrice) / avgBuyPrice)*100,2);

            companyObject.transform.Find("companyNameText").GetComponent<TextMeshProUGUI>().text = companyName;
            companyObject.transform.Find("boughtShresText").GetComponent<TextMeshProUGUI>().text = nShares.ToString();
            companyObject.transform.Find("avgBuyPriceText").GetComponent<TextMeshProUGUI>().text = avgBuyPrice.ToString();
            companyObject.transform.Find("sellPriceText").GetComponent<TextMeshProUGUI>().text = price_now.ToString();
            companyObject.transform.Find("changeText").GetComponent<TextMeshProUGUI>().text = $"{change}%";

            if (change > 0)
            {
                companyObject.transform.Find("changeText").GetComponent<TextMeshProUGUI>().color = new Color32(0,130,0,255);
            }
            else if(change < 0)
            {
                companyObject.transform.Find("changeText").GetComponent<TextMeshProUGUI>().color = new Color32(255,0,0,255);
            }
        }

        dbManager.CloseConnection();
    }

    public void getDetails(SC_Company selectedCompany)
    {
        Debug.Log(selectedCompany.companyName);
    }

    public void clearList(GameObject list)
    {
        foreach (Transform objectInList in list.transform)
        {
            if (objectInList.name == "List title") continue;
            else
            {
                GameObject.Destroy(objectInList.gameObject);
            }
        }
    }
}
