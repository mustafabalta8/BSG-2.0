using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SC_Company : MonoBehaviour
{
    StockExchange stockExchange;
    BoughtShares boughtShares;
    DbManager dbManager;

    public string companyName;
    public int brandValue, companyPower, budget, fans, price, shares;

    public void buyShares()
    {
        stockExchange = FindObjectOfType<StockExchange>();

        stockExchange.buyShares(this);
    }

    public void updateCompany()
    {
        dbManager = FindObjectOfType<DbManager>();

        string query = $"UPDATE all_companies SET " +
                        $"companyName = \"{this.companyName}\", " +
                        $"brandValue= {this.brandValue}, " +
                        $"companyPower = {this.companyPower}, " +
                        $"budget = {this.budget}, " +
                        $"fans = {this.fans}, " +
                        $"shares = {this.shares} " +
                        $"WHERE companyName = \"{this.companyName}\"";
        dbManager.ReadRecords(query);
        dbManager.CloseConnection();
    }

    public SC_Company(string comp)
    {
        dbManager = FindObjectOfType<DbManager>();

        string query = string.Format($"SELECT * FROM all_companies WHERE companyName = \"{comp}\"");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int brandValue = reader.GetInt32(1);
            int companyPower = reader.GetInt32(2);
            int budget = reader.GetInt32(3);

            price = ((budget + (companyPower / brandValue)) / 100);
        }
        dbManager.CloseConnection();
    }

    public int getStockPrice()
    {
        return price;
    }

    public void getBoughtShareDetails()
    {
        boughtShares = FindObjectOfType<BoughtShares>();
        boughtShares.getDetails(this);
    }
}
