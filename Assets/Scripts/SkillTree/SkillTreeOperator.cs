using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;

public class SkillTreeOperator : MonoBehaviour
{

    public string SkillName;
    public int level;
    public int cost;
    public int feature;
    public string featureName;

    DbManager dbManager;
    MoneyManager moneyManager;

    [SerializeField] GameObject SkillsPanel;

     string SkillName0;
     int level0;
     int cost0;
     int feature0;
     string featureName0;

    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        moneyManager = FindObjectOfType<MoneyManager>();

        //arrangeSkillsToLevel1();

       DownloadSkillInfo();


    }
    void arrangeSkillsToLevel1()//for restart
    {
        

        string query = "SELECT * FROM skills WHERE level='1'";
        IDataReader reader8 = dbManager.ReadRecords(query);

        while (reader8.Read())
        {

            string skillName = reader8.GetString(0);
            
            switch (skillName)
            {
                case "thrifty":
                    fillSkillDetails0( reader8,"thrifty");
                    break;
                case "chafferer":
                    fillSkillDetails0(reader8, "chafferer");
                    break;
                case "decorator":
                    fillSkillDetails0(reader8, "decorator");
                    break;
                case "fertile":
                    fillSkillDetails0(reader8, "fertile");
                    break;
                case "peaceful atmosphere":
                    fillSkillDetails0(reader8, "peaceful atmosphere");
                    break;

                default:
                    break;
            }
        
        }
        dbManager.CloseConnection();
    }
    void fillSkillDetails0(IDataReader reader, string nameSkill)
    {

            level0 = reader.GetInt32(1);
            featureName0 = reader.GetString(2);
            feature0 = reader.GetInt32(3);
            cost0 = reader.GetInt32(4);

        dbManager.CloseConnection();
        string query02 = string.Format("UPDATE presented_skills SET level ='{0}', featureName='{1}',feature='{2}',cost='{3}' WHERE skillName= '{4}'", level0, featureName0, feature0, cost0,nameSkill);
        dbManager.InsertRecords(query02);
    }
    void DownloadSkillInfo()
    {
        string query = "SELECT * FROM presented_skills";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {

            string skillName = reader.GetString(0);

            switch (skillName)
            {
                case "thrifty":
                    fillSkillDetails(1, reader);
                    break;
                case "chafferer":
                    fillSkillDetails(2, reader);
                    break;
                case "decorator":
                    fillSkillDetails(3, reader);
                    break;
                case "fertile":
                    fillSkillDetails(4, reader);
                    break;
                case "peaceful atmosphere":
                    fillSkillDetails(5, reader);
                    break;

                default:
                    break;
            }

        }
        dbManager.CloseConnection();
    }

    public void LevelUpgrade()
    {
        //if(cost<=money)
        //moneyManager.changeMoney(-cost,SkillName);

        //activate the feature


        //read next level 
        //string query = $"SELECT * FROM skills WHERE skillName = {SkillName} AND level = {level+1}";
        string query = string.Format("SELECT * FROM skills WHERE skillName = '{0}' AND level = {1}", SkillName, level+1);
        IDataReader reader = dbManager.ReadRecords(query);
        while (reader.Read())
        {
            level = reader.GetInt32(1);
            featureName = reader.GetString(2);
            feature = reader.GetInt32(3);
            cost = reader.GetInt32(4);
        }

        //change the presented_skilss table in DB
        //string query02 = $"UPDATE presented_skills SET level ={level}, featureName={featureName},feature={feature},cost={cost} WHERE skillName={SkillName}";
        string query02 = string.Format("UPDATE presented_skills SET level ='{0}', featureName='{1}',feature='{2}',cost='{3}' WHERE skillName='{4}'",level,featureName,feature,cost,SkillName);
        dbManager.InsertRecords(query02);

        dbManager.CloseConnection();


        DownloadSkillInfo();
    }

    private void fillSkillDetails(int skillName, IDataReader reader)
    {
        SkillsPanel.transform.Find($"Office/Skill {skillName}").GetComponent<Skill>().SkillName = reader.GetString(0);
        SkillsPanel.transform.Find($"Office/Skill {skillName}").GetComponent<Skill>().level = reader.GetInt32(1);
        SkillsPanel.transform.Find($"Office/Skill {skillName}").GetComponent<Skill>().FeatureName = reader.GetString(2);
        SkillsPanel.transform.Find($"Office/Skill {skillName}").GetComponent<Skill>().feature = reader.GetInt32(3);
        SkillsPanel.transform.Find($"Office/Skill {skillName}").GetComponent<Skill>().cost = reader.GetInt32(4);
    }

   public void ChangeInspectorInterface()
    {

    }
}
