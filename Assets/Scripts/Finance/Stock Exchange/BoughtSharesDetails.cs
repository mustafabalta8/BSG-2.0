using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoughtSharesDetails : MonoBehaviour
{
    DbManager dbManager;
    MoneyManager moneyManager;
    SC_Company SC_company;
    BoughtShares boughtShares;

    [SerializeField] GameObject detailsPanel;

    [SerializeField] GameObject AllSharesUI;
    [SerializeField] GameObject ShareObject;

    [SerializeField] TextMeshProUGUI companyNameText;
    [SerializeField] Button sellButton;

    public int shareId, sellPrice;
    public string companyName;

    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        SC_company = FindObjectOfType<SC_Company>();
        boughtShares = FindObjectOfType<BoughtShares>();
    }

    public void getShares(string companyName)
    {
        clearList(AllSharesUI);

        companyNameText.text = companyName;

        string query = string.Format($"SELECT * FROM bought_shares WHERE company_name = \"{companyName}\" ORDER BY time ASC");
        IDataReader reader = dbManager.ReadRecords(query);

        SC_Company createdBoughtCompany = new SC_Company(companyName);
        sellPrice = createdBoughtCompany.getStockPrice();

        while (reader.Read())
        {
            //GameObject createdCompany = Instantiate(ShareObject);
            //createdCompany.transform.SetParent(AllSharesUI.transform);
            GameObject createdCompany = Instantiate(ShareObject, AllSharesUI.transform,false);

            BoughtSharesDetails boughtShare = createdCompany.GetComponent<BoughtSharesDetails>();

            double change = Math.Round(((sellPrice - (double)reader.GetInt32(3)) / reader.GetInt32(3)) * 100, 2);

            boughtShare.shareId = reader.GetInt32(0);
            boughtShare.sellPrice = sellPrice;
            boughtShare.companyName = companyName;

            boughtShare.transform.Find("buyingTimeText").GetComponent<TextMeshProUGUI>().text = reader.GetInt32(2).ToString();
            boughtShare.transform.Find("buyingPriceText").GetComponent<TextMeshProUGUI>().text = $"{reader.GetInt32(3)}$";
            boughtShare.transform.Find("priceNowText").GetComponent<TextMeshProUGUI>().text = $"{sellPrice}$";
            boughtShare.transform.Find("changeText").GetComponent<TextMeshProUGUI>().text = $"{change}%";

            boughtShare.name = shareId.ToString();
        }

        dbManager.CloseConnection();


        detailsPanel.SetActive(true);
    }

    public void closePanel()
    {
        clearList(AllSharesUI);

        companyNameText.text = "";

        detailsPanel.SetActive(false);
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

    public void sellShare()
    {
        int companyShares = 100;
        string query = $"DELETE FROM bought_shares WHERE id = {shareId}";
        dbManager.ReadRecords(query);

        query = string.Format($"SELECT shares FROM all_companies WHERE companyName = \"{companyName}\"");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            companyShares = reader.GetInt32(0);
        }

        query = $"UPDATE all_companies SET shares = {companyShares+1} WHERE companyName = \"{companyName}\"";
        dbManager.ReadRecords(query);

        dbManager.CloseConnection();

        boughtShares.getAllBoughtShares();
        //getShares(companyName);

        moneyManager.changeMoney(sellPrice, "Stock exchange");
    }
}
