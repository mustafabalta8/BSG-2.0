using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;

public class Skill : MonoBehaviour
{
    public string skillName, featureName, type;

    public int level, cost, feature;

    public bool active = false;
   int boughtLevel = 0;

    [SerializeField] Button skillButton;

    SkillTreeOperator skillTreeOperator;
    DbManager dbManager;

    [SerializeField] GameObject SkillInspector;
    // Start is called before the first frame update
    void Start()
    {
        skillTreeOperator = FindObjectOfType<SkillTreeOperator>();
        dbManager = FindObjectOfType<DbManager>();

        skillName = this.name;
        type = this.transform.parent.name;

        fillButton();
    }

    public void fillButton()
    {
        bool bought = false;

        string query = $"SELECT count(level) as nActive, level FROM skills WHERE skillName = \"{skillName}\" AND active = 1 ORDER BY level DESC LIMIT 1";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            if(reader.GetInt32(0) > 0)
            {
                bought = true;
                boughtLevel = reader.GetInt32(1);
            }
        }
        dbManager.CloseConnection();

        if (bought)
        {
            query = $"SELECT level FROM skills WHERE skillName = \"{skillName}\" AND active = 1 ORDER BY level DESC LIMIT 1";
            reader = dbManager.ReadRecords(query);

            while (reader.Read())
            {
                if (boughtLevel == 3)
                {
                    skillButton.interactable = false;
                    this.transform.Find("level").GetComponent<Text>().text = $"Level: 3";
                }
                else
                {
                    this.transform.Find("level").GetComponent<Text>().text = $"Level: {reader.GetInt32(0)+1}";
                }
            }
            dbManager.CloseConnection();
        }
        else
        {
            this.transform.Find("level").GetComponent<Text>().text = $"Level: 1";
        }
    }

    public void showInfo()
    {
        int cost = 0;

        string query = $"SELECT * FROM skills WHERE skillName = \"{skillName}\" AND level = {boughtLevel+1} ORDER BY level DESC LIMIT 1";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            SkillInspector.transform.Find("Skill name").GetComponent<TextMeshProUGUI>().text = skillName + $" Level {boughtLevel+1}";
            SkillInspector.transform.Find("Panel/Furniture").GetComponent<TextMeshProUGUI>().text = $"Service: {reader.GetString(4)}";
            SkillInspector.transform.Find("Panel/Cost").GetComponent<TextMeshProUGUI>().text = $"Cost: ${reader.GetInt32(6)}";
            cost = reader.GetInt32(6);
        }
        dbManager.CloseConnection();

        skillTreeOperator.cost = cost;
        skillTreeOperator.skillName = skillName;
        skillTreeOperator.skillType = type;
        skillTreeOperator.level = boughtLevel + 1;
        skillTreeOperator.skill = this;
    }
}
