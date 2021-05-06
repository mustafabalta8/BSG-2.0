using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ads : MonoBehaviour
{
    private MoneyManager moneyManager;
    private TimeManager timeManager;
    private DbManager dbManager;


    private string selectedProduct;
    private int startTime, endTime, duration, budget;
    private bool campaignStarted = false;

    [SerializeField] TMP_Dropdown productDropDown;
    [SerializeField] Slider durationSlider;
    [SerializeField] Slider budgetSlider;
    [SerializeField] Button startButton;

    [SerializeField] TMP_Text durationText;
    [SerializeField] TMP_Text budgetText;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        dbManager = FindObjectOfType<DbManager>();

        setBudget();
        durationText.text = "0";

        durationSlider.onValueChanged.AddListener(delegate {
            durationText.text = $"{durationSlider.value}"; //changes only once? why?
        });

        budgetSlider.onValueChanged.AddListener(delegate {
            budgetText.text = $"{budgetSlider.value}$"; //changes only once? why?
        });
    }
    private void Update()
    {
        checkCampaign();
    }

    void setBudget()
    {
        budgetSlider.minValue = 100;
        budgetSlider.maxValue = moneyManager.money;

        budgetText.text = $"{budgetSlider.minValue}$";
    }

    private void startAdCampaign()
    {
        selectedProduct = productDropDown.options[productDropDown.value].text;
        duration = (int)durationSlider.value;
        budget = (int)budgetSlider.value;

        moneyManager.changeMoney(budget * -1);

        startTime = timeManager.displayTime;

        campaignStarted = true;

        startButton.interactable = false;
        Debug.Log($"campaign started, will be finished on {startTime+duration}");
    }

    private void endAdCampaign()
    {
        string query = $"INSERT INTO ads VALUES (null,\"{selectedProduct}\",{budget},{duration},{startTime},{startTime + duration})";
        dbManager.ReadRecords(query);
        dbManager.CloseConnection();

        campaignStarted = false;
        startButton.interactable = false;

        Debug.Log("campaign finished, added to db");
    }

    private void checkCampaign()
    {
        if (campaignStarted)
        {
            if (timeManager.displayTime >= (startTime + duration))
            {
                endAdCampaign();
            }
            else
            {
                return;
            }
        }
    }
}
