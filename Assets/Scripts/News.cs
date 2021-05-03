using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    DbManager dbManager;

    // [SerializeField] Text newsText;
    [SerializeField] GameObject newsPanel;
    [SerializeField] GameObject newsObj;

    int interval = 1;
    List<int> news_ids = new List<int>();

    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();

        StartCoroutine(StartNewsFeed());
    }

    IEnumerator StartNewsFeed()
    {
        int news_this_round = 0;
        while (true)
        {

            news_ids.ForEach(delegate (int news_id) {
                DestroyNews(news_id);
            });

            news_ids.Clear();

        string query = string.Format("SELECT * FROM news ORDER BY RANDOM()");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int news_id = reader.GetInt32(0);
            string title = reader.GetString(1);
            string news = reader.GetString(2);
            string newsPaper = reader.GetString(3);

            if (!news_ids.Contains(news_id))
            {
                news_ids.Add(news_id);

                GameObject CreateNews = Instantiate(newsObj);
                CreateNews.transform.SetParent(newsPanel.transform);

                CreateNews.transform.Find("NewsTitle").GetComponent<Text>().text = title;
                CreateNews.transform.Find("NewsText").GetComponent<Text>().text = news;
                CreateNews.transform.Find("NewsPaper").GetComponent<Text>().text = newsPaper;

                CreateNews.name = string.Format("[{0}]",news_id);

                news_this_round++;

                    if (news_this_round >= 2)
                    {
                        break;
                    }
            }
        }
        dbManager.CloseConnection();
        
        news_this_round = 0;

        yield return new WaitForSeconds(interval*10);
        }
    }
    public void DestroyNews(int news_id)
    {
        Destroy(newsPanel.transform.Find(string.Format("[{0}]", news_id)).gameObject);
    }
}
