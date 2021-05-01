using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    TimeManager timeManager;
    int currentTime;

    // [SerializeField] Text newsText;
    [SerializeField] GameObject newsPanel;
    [SerializeField] GameObject newsObj;
    string[] newsSt = { "Merhabaagh Merhabaagh Merhabaagh Merhabaagh Merhabaagh", "sad" ,"new 1","new 2"};

    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        CreateNews();
      //  currentTime = timeManager.displayTime;

    }

   public void CreateNews()
    {
        int turn = i;
        while (i < newsSt.Length)
        {
            
            GameObject CreateNews = Instantiate(newsObj);
            CreateNews.transform.SetParent(newsPanel.transform);
            //CreateNews.name = i.ToString();

            CreateNews.transform.Find("NewsText").GetComponent<Text>().text = newsSt[i];
            i++;
            if (turn + 2 == i)
                break;
        }
        
    }
    public void DestroyNews()
    {
        int count = 0;
        while (count<6)
        {
            //string obsName = count.ToString();
            Destroy(newsPanel.transform.Find("NewsObj(Clone)").gameObject);//why count.ToString() not work in Find()
            count++;
        }
        
    }
}
