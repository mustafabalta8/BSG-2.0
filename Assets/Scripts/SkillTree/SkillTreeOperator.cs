using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using UnityEngine.UI;

public class SkillTreeOperator : MonoBehaviour
{
    public int level, cost;
    public string skillName, skillType;
    public Skill skill;
    [SerializeField] Button buyButton;


    DbManager dbManager;
    MoneyManager moneyManager;
    Notifications notificationManager;
    Company company;

    [SerializeField] GameObject SkillsPanel;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        notificationManager = FindObjectOfType<Notifications>();
        company = FindObjectOfType<Company>();
    }

    public void buySkill()
    {
        if (skillName == "dream team")
        {
            if (level == 3)
            {
                notificationManager.pushNotification("You can not buy this skill in the first office. Maximum allowed staff is 4.");
                buyButton.interactable = false;
                return;
            }
        }

        if (moneyManager.getBalance() < cost)
        {
            notificationManager.pushNotification("You don't enough money to buy this skill.");
            return;
        }

        try
        {
            dbManager = FindObjectOfType<DbManager>();
            string query = $"UPDATE skills SET active = 0 WHERE skillName = \"{skillName}\"; UPDATE skills SET active = 1 WHERE skillName = \"{skillName}\" AND level = {level};";
            dbManager.InsertRecords(query);
            dbManager.CloseConnection();

            notificationManager.pushNotification($"{skillName} level {level} is activated.");
            skill.fillButton();

            buyButton.interactable = false;
        }
        catch (System.Exception error)
        {
            Debug.Log(error);
            throw;
        }

        company.getSkills();
    }
}
