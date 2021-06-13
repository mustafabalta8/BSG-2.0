using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BackgroundWorker : MonoBehaviour
{
    private MoneyManager moneyManager;
    private TimeManager timeManager;
    private DbManager dbManager;
    private MusicManager soundManager;

    int turnNow;

    bool moneyChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        dbManager = FindObjectOfType<DbManager>();
        soundManager = FindObjectOfType<MusicManager>();

        turnNow = timeManager.displayTime;
    }

    // Update is called once per frame
    void Update()
    {
        checkWorkers();
    }

    public void productWorker()
    {
        int totalMoneyMade = 0;
        List<int> product_ids = new List<int>();

        string query = string.Format("SELECT id FROM products");

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            product_ids.Add(reader.GetInt32(0));
        }
        dbManager.CloseConnection();

        foreach (var product_id in product_ids)
        {
            Product product = new Product(product_id);
            int sellAmount = product.getSellAmount();
            int price = product.price;

            query = string.Format($"INSERT INTO products_sold VALUES({product_id},{sellAmount},{timeManager.displayTime})");
            dbManager.InsertRecords(query);

            moneyManager.changeMoney(price * sellAmount, "Product sell");

            totalMoneyMade += price * sellAmount;
        }


        if (totalMoneyMade > 0) moneyChanged = true;
        Debug.Log("total money made: " + totalMoneyMade);
    }

    public void salaryWorker()
    {
        int totalSalary = 0;
        string query = string.Format("SELECT SUM(employeeSalary) FROM employees WHERE hired = 1");

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            totalSalary = reader.GetInt32(0);
        }
        dbManager.CloseConnection();

        if (totalSalary > 0)
        {
            moneyChanged = true;
            moneyManager.changeMoney(-totalSalary, "Salary");
        }
    }

    public void officeRenWorker()
    {
        int rent = 0;
        string query = string.Format("SELECT rent FROM office"); //how to see which office is active?

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            rent = reader.GetInt32(0);
        }
        dbManager.CloseConnection();

        if (rent > 0)
        {
            moneyChanged = true;
            moneyManager.changeMoney(-rent, "Rent");
        }

    }

    public void startWorkers()
    {
        productWorker();
        salaryWorker();
        officeRenWorker();
    }

    public void checkWorkers()
    {
        if (turnNow == timeManager.displayTime) return;
        else
        {
            if (timeManager.displayTime % 30 == 0)
            {
                startWorkers();
                turnNow = timeManager.displayTime;
                if (moneyChanged) soundManager.playSound("money");
            }
            else
            {
                return;
            }
        }

    }
}
