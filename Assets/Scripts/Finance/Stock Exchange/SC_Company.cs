using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Company : MonoBehaviour
{
    StockExchange stockExchange;
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
}
