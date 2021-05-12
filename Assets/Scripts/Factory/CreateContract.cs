using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using TMPro;

public class CreateContract : MonoBehaviour
{
    [SerializeField] string[] platform = { "Desktop","Mobile","Console" };
    [SerializeField] string[] type = { "Game", "App", "Web" };
    string companyName= "Fahrettin";

    DbManager dbManager;
    [SerializeField] GameObject ContractObj;
    [SerializeField] GameObject ContractPanel;
    /*�irket bilgisi veritaban�ndan al�nmal�. o �irket a��rl�kl� olarak ne ile u�ra��yorsa ona y�nelik bir contract i� olu�turulmal�. */
    //contract work'den kazan� �irketin gelirine g�re de�i�meli �irket �ok kazan�yorken daha b�t�k i�ler gelmeli, k���kken k���k i�ler gelmeli
    public List<Contract> Contracts = new List<Contract>();
    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();

        CreateContracts();
    }
    void CreateContracts()
    {
        Contracts.Clear();
        int i = 0;
        while (i < 9)
        {
            GameObject newProduct = Instantiate(ContractObj);
            newProduct.transform.SetParent(ContractPanel.transform);

            Contract contractObj = newProduct.GetComponent<Contract>();

            Contracts.Add(contractObj);

            //contractObj.productId = ;
            contractObj.companyName = companyName;
            contractObj.platform = platform[Random.Range(0, 2)];
            contractObj.sofType = type[Random.Range(0, 2)];
            contractObj.duration = Random.Range(5, 18);
            contractObj.offer = contractObj.duration * 125;

            contractObj.transform.Find("company").GetComponent<TextMeshProUGUI>().text = companyName;
            contractObj.transform.Find("type").GetComponent<TextMeshProUGUI>().text = contractObj.sofType;
            contractObj.transform.Find("platform").GetComponent<TextMeshProUGUI>().text = contractObj.platform;
            contractObj.transform.Find("duration").GetComponent<TextMeshProUGUI>().text = contractObj.duration.ToString();
            contractObj.transform.Find("offer").GetComponent<TextMeshProUGUI>().text = contractObj.offer.ToString();

            i++;
        }
        
            





    }

}
