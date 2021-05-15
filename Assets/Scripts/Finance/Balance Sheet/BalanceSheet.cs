using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class BalanceSheet : MonoBehaviour
{
    DbManager dbManager;
    TimeManager timeManager;
    MoneyManager moneyManager;

    [SerializeField] Text assets_cashText, assets_contractsText, assets_featuresText, assets_inventoryText, assets_total_current_assetsText, assets_totalText;
    [SerializeField] Text liabilities_inventoryText, liabilities_adsText, liabilities_manText, liabilities_trainingText, liabilities_taxesText, liabilities_totalText;

    [SerializeField] Text turnViewer;


    [SerializeField] Button nextTurnButton, previousTurnButton;


    int liabilities_inventory, liabilities_ads, liabilities_man, liabilities_training, liabilities_taxes, liabilities_total = 0;
    int assets_contracts, assets_features, assets_inventory, assets_total_current_assets, assets_total = 0;

    private int turn;

    private void Start()
    {

        dbManager = FindObjectOfType<DbManager>();
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();

        turn = timeManager.displayTime;

        getData(turn);

        initiateButtons();
    }

    void updateAssetsText()
    {
        assets_cashText.text = moneyManager.money.ToString();
        assets_contractsText.text = assets_contracts.ToString(); ;
        assets_featuresText.text = assets_features.ToString();
        assets_inventoryText.text = assets_inventory.ToString();
        assets_total_current_assetsText.text = assets_total_current_assets.ToString();
        assets_totalText.text = assets_total.ToString();
    }

    void updateLiabilitiesText()
    {
        liabilities_inventoryText.text = liabilities_inventory.ToString();
        liabilities_adsText.text = liabilities_ads.ToString();
        liabilities_manText.text = liabilities_man.ToString();
        liabilities_trainingText.text = liabilities_training.ToString();
        liabilities_taxesText.text = liabilities_taxes.ToString();
        liabilities_totalText.text = liabilities_total.ToString();
    }
    
    void updateAssets(int time)
    {
        string query = string.Format($"SELECT * FROM bank_transactions " +
                                    $"WHERE (type = \"Contract\" " +
                                    $"OR type = \"Features\" " +
                                    $"OR type = \"Inventory\" " +
                                    $"OR type = \"Other\")" +
                                    $"AND `transaction` > 0 " +
                                    $"AND time = {time}");

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            if(reader.GetString(2) == "Contract")
            {
                assets_contracts = reader.GetInt32(1);
                assets_total += assets_contracts;
            }
            else if (reader.GetString(2) == "Features")
            {
                assets_features = reader.GetInt32(1);
                assets_total += assets_features;
            }
            else if (reader.GetString(2) == "Inventory")
            {
                assets_inventory = reader.GetInt32(1);
                assets_total += assets_inventory;
            }
            else
            {
                assets_total_current_assets = reader.GetInt32(1);
                assets_total += assets_total_current_assets;
            }
        }

        dbManager.CloseConnection();

        assets_total += moneyManager.money;

        updateAssetsText();
    }
    void updateLiabilities(int time)
    {
        string query = string.Format($"SELECT * FROM bank_transactions " +
                                    $"WHERE (type = \"Inventory\" " +
                                    $"OR type = \"Ads\" " +
                                    $"OR type = \"Management\" " +
                                    $"OR type = \"Salary\" " +
                                    $"OR type = \"Training\" " +
                                    $"OR type = \"Tax\") " +
                                    $"AND `transaction` < 0 " +
                                    $"AND time = {time}");

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            if (reader.GetString(2) == "Inventory")
            {
                liabilities_inventory = reader.GetInt32(1);
                liabilities_total += liabilities_inventory;
            }
            else if (reader.GetString(2) == "Ads")
            {
                liabilities_ads = reader.GetInt32(1);
                liabilities_total += liabilities_ads;
            }
            else if (reader.GetString(2) == "Management")
            {
                liabilities_man = reader.GetInt32(1);
                liabilities_total += liabilities_man;
            }
            else if (reader.GetString(2) == "Salary")
            {
                liabilities_man = reader.GetInt32(1);
                liabilities_total += liabilities_man;
            }
            else if (reader.GetString(2) == "Training")
            {
                liabilities_training = reader.GetInt32(1);
                liabilities_total += liabilities_training;
            }
            else if (reader.GetString(2) == "Tax")
            {
                liabilities_taxes = reader.GetInt32(1);
                liabilities_total += liabilities_taxes;

            }
        }

        dbManager.CloseConnection();

        updateLiabilitiesText();
    }

    public void previousTurn()
    {
        if (turn > 0)
        {
            previousTurnButton.interactable = true;
            turn--;
        }
        else
        {
            previousTurnButton.interactable = false;
        }

        getData(turn);

        initiateButtons();
    }

    public void nextTurn()
    {
        turn++;

        getData(turn);

        initiateButtons();
    }

    void initiateButtons()
    {
        if (timeManager.displayTime <= turn)
        {
            //nextTurnButton.interactable = false;
        }
        else
        {
            //nextTurnButton.interactable = true;
        }
    }

    void getData(int turn)
    {
        turnViewer.text = turn.ToString();

        liabilities_inventory = 0;
        liabilities_ads = 0;
        liabilities_man = 0;
        liabilities_training = 0;
        liabilities_taxes = 0;
        liabilities_total = 0;
        assets_contracts = 0;
        assets_features = 0;
        assets_inventory = 0;
        assets_total_current_assets = 0;
        assets_total = 0;


        updateAssets(turn);
        updateLiabilities(turn);
    }
}
