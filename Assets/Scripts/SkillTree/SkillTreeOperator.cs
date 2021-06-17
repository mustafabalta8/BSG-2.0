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

    [SerializeField] GameObject SkillsPanel;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        notificationManager = FindObjectOfType<Notifications>();
    }

    public void buySkill()
    {
        if(moneyManager.getBalance() < cost)
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

    }
}
