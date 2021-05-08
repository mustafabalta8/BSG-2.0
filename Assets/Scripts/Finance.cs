using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;

public class Finance : MonoBehaviour
{

    /*
     * credibility rating:
     * 0 - 2 -> unacceptable -> reject
     * 2 - 4 -> questionable -> up to 1x bank balance, 0.10 interest rate
     * 4 - 6 -> good -> up to 2x  bank balance, 0.07 interest rate
     * 6 - 8 -> very good -> up to 4x  bank balance, , 0.05 interest rate
     * 8 - 10 -> excellent -> unlimited, , 0.03 interest rate
     */


    private MoneyManager moneyManager;
    private TimeManager timeManager;
    private DbManager dbManager;
    private Company company;

    [Header("Credit Related")]
    [SerializeField] TMP_InputField creaditAmount;
    [SerializeField] Slider durationSlider;

    [SerializeField] Text durationText, credibilityValue, totalAmount, paybackAmountPerPeriod, amountFeedback, durationFeedback, interestRate, totalPayback;

    [SerializeField] Button approveAmountButton, approvePeriodButton, takeCreditButton;

    private int payBack, credibility, requestedAmount, payback;

    private bool creditTaken = false;
    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        dbManager = FindObjectOfType<DbManager>();
        company = FindObjectOfType<Company>();

        company.getCompanyDataFromDatabase();
        credibility = company.credibility;
        credibilityValue.text = company.credibility.ToString();

        durationText.text = "0";

        totalAmount.text = "-";
        interestRate.text = "-";
        totalPayback.text = "-";
        paybackAmountPerPeriod.text = "-";


        durationSlider.onValueChanged.AddListener(delegate {
            durationText.text = $"{durationSlider.value}";
        });

        disableButtons();
    }

    private void disableButtons()
    {
        durationSlider.interactable = false;
        approvePeriodButton.interactable = false;

        takeCreditButton.interactable = false;
    }

    public void approveAmount()
    {
        requestedAmount = int.Parse(creaditAmount.text);

        if (credibility > 2)
        {
            if((credibility >= 2) && (credibility <= 4))
            {
                if (requestedAmount <= moneyManager.money * 1)
                {
                    amountFeedback.text = "Amount accepted";
                }
                else
                {
                    amountFeedback.text = $"Due to your credibility value, you can request up to ${moneyManager.money * 1} credit.";
                    disableButtons(); return;
                }
            }
            else if((credibility >= 4) && (credibility <= 6))
            {
                if (requestedAmount <= moneyManager.money * 2)
                {
                    amountFeedback.text = "Amount accepted";
                }
                else
                {
                    amountFeedback.text = $"Due to your credibility value, you can request up to ${moneyManager.money * 2} credit.";
                    disableButtons(); return;
                }
            }
            else if((credibility >= 6) && (credibility <= 8))
            {
                if (requestedAmount <= moneyManager.money * 4)
                {
                    amountFeedback.text = "Amount accepted";
                }
                else
                {
                    amountFeedback.text = $"Due to questionable credibility value, you can request up to ${moneyManager.money * 4} credit.";
                    disableButtons(); return;
                }
            }
            else
            {
                amountFeedback.text = "Amount accepted";
            }
        }
        else
        {
            amountFeedback.text = "unacceptable credibility";
            return;
        }

        approvePeriodButton.interactable = true;
        durationSlider.interactable = true;
    }

    public void calculatePayback()
    {
        takeCreditButton.interactable = true;
        double interest;

        if ((credibility >= 2) && (credibility <= 4))
        {
            interest = 0.5;
        }
        else if ((credibility >= 4) && (credibility <= 6))
        {
            interest = 0.3;
        }
        else if ((credibility >= 6) && (credibility <= 8))
        {
            interest = 0.2;
        }
        else
        {
            interest = 0.1;
        }

        payBack = (int)(int.Parse(creaditAmount.text) * (1 + interest));
        int paybackPerMonth = (int)(payBack / durationSlider.value);

        totalAmount.text = $"{creaditAmount.text}";
        interestRate.text = $"{interest}%";
        totalPayback.text = $"{payBack}";
        paybackAmountPerPeriod.text = $"{paybackPerMonth}";
    }

    public void takeCredit()
    {
        creditTaken = true;


    }

}
