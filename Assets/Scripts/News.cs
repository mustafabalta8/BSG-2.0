using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    DbManager dbManager;

    [SerializeField] GameObject newsPanel;
    [SerializeField] GameObject newsObj;

    int interval = 1; // # of turns for the newsfeed
    List<int> news_ids = new List<int>(); //ids of news shown in the current turn

    void Start()
    {
        dbManager = FindObjectOfType<DbManager>(); // get db manager to read news from db

        StartCoroutine(StartNewsFeed()); // start the newsfeed, this initiates the method and keeps in a loop
    }

    //this is quite inefficient because it connects to the dband reads and disconnects, IN A CONTINOUS LOOP...
    //i should just read the news once and store it somewhere in a list, then loop though it but if it aint broke dont fix :)
    IEnumerator StartNewsFeed()
    {
        int news_this_round = 0; // how manu news read from the database, accepted and shown in the newsfeed
        while (true) //keep a loop
        {

            news_ids.ForEach(delegate (int news_id) { //delete news from previous rounds, if any
                DestroyNews(news_id); // destroy the given news
            });

            news_ids.Clear(); // clear the list that keeps news shown in the current turn

        string query = string.Format("SELECT * FROM news ORDER BY RANDOM()"); // get news from db in a random order
        IDataReader reader = dbManager.ReadRecords(query); // execute query

        while (reader.Read()) // while query return results..
        {
            // get values from the read record into a variable
            int news_id = reader.GetInt32(0);
            string title = reader.GetString(1);
            string news = reader.GetString(2);
            string newsPaper = reader.GetString(3);

            if (!news_ids.Contains(news_id)) // if the read news from the db is NOT in the current shown news in the newsfeed...
            {
                news_ids.Add(news_id); // add the id to the current shown news in the newsfeed

                GameObject CreateNews = Instantiate(newsObj); // create news object
                CreateNews.transform.SetParent(newsPanel.transform); // set the created objects' parent as newsPanel

                //TODO: ok, this part is quite buggy, needs some unity black magic
                CreateNews.transform.Find("NewsTitle").GetComponent<Text>().text = title; //print the news title
                CreateNews.transform.Find("NewsText").GetComponent<Text>().text = news; //print the news
                CreateNews.transform.Find("NewsPaper").GetComponent<Text>().text = newsPaper; //print the newspaper name

                CreateNews.name = string.Format("[{0}]",news_id); // change the name of the news object created to its id, makes it much easier to manage and destroy in the future

                news_this_round++; // add a new news for this round

                    if (news_this_round >= 2) // if there are more than 2 news in the current newsfeed then...
                    {
                        break; // break the loop of reading, thus no more news
                    }
            }
        }
        dbManager.CloseConnection(); // close the connection to the db
        
        news_this_round = 0; // set this to 0 for the next rount

        yield return new WaitForSeconds(interval*10); // sleep for given amound of time
        }
    }
    public void DestroyNews(int news_id) // destroys the news by its news id
    {
        Destroy(newsPanel.transform.Find(string.Format("[{0}]", news_id)).gameObject); // find and destroy the gameobject with the name of given parameter within the newspanel
    }
}
