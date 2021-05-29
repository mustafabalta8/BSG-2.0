using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Skill : MonoBehaviour
{
    public string SkillName;
    public string FeatureName;

    public int level;
    public int cost;
    public int feature;

    SkillTreeOperator skillTreeOperator;

    [SerializeField] GameObject SkillInspector;
    // Start is called before the first frame update
    void Start()
    {
        skillTreeOperator = FindObjectOfType<SkillTreeOperator>();
    }

    public void showInfo()
    {
        
        SkillInspector.transform.Find("Skill name").GetComponent<TextMeshProUGUI>().text = SkillName + " Level "+level;
        SkillInspector.transform.Find("Panel/Furniture").GetComponent<TextMeshProUGUI>().text =FeatureName;
        SkillInspector.transform.Find("Panel/Cost").GetComponent<TextMeshProUGUI>().text = "Cost: "+cost;


        skillTreeOperator.SkillName = SkillName;
        skillTreeOperator.level = level;
        skillTreeOperator.cost = cost;
        skillTreeOperator.feature = feature;

    }

    
}
