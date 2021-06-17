using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using TMPro;


public class ListProduct : MonoBehaviour
{
    DbManager dbManager;
    [SerializeField] GameObject ProductUI;
    [SerializeField] GameObject ProductUIPanel;

    public List<Product> Products = new List<Product>();
    // Start is called before the first frame update
    void Start()
    {
        dbManager = FindObjectOfType<DbManager>();

        getProductsFromDatabase();
    }
    void getProductsFromDatabase()
    {

        Products.Clear();

        string query = string.Format("SELECT * FROM products ");
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int productId = reader.GetInt32(0);
            string productName = reader.GetString(1);
            string type = reader.GetString(2);
            string platform = reader.GetString(3);
            int quality = reader.GetInt32(9);
           

            //GameObject newProduct = Instantiate(ProductUI);
            //newProduct.transform.SetParent(ProductUIPanel.transform);
            GameObject newProduct = Instantiate(ProductUI, ProductUIPanel.transform,false);

            Product productObj = newProduct.GetComponent<Product>();

            Products.Add(productObj);

            productObj.productId = productId;
            productObj.productName = productName;
            productObj.softwareType = type;
            productObj.platform = platform;
            productObj.quality = quality;

            productObj.name = productName.ToString();


            productObj.transform.Find("name").GetComponent<TextMeshProUGUI>().text = productName;
            productObj.transform.Find("type").GetComponent<TextMeshProUGUI>().text = type;
            productObj.transform.Find("platform").GetComponent<TextMeshProUGUI>().text = platform;
            productObj.transform.Find("quality").GetComponent<TextMeshProUGUI>().text = quality.ToString();

            string query_ind = string.Format($"SELECT SUM(amountSold) FROM products_sold WHERE productId = {productId}");
            IDataReader reader_ind = dbManager.ReadRecords(query_ind);

            while (reader_ind.Read())
            {
                productObj.transform.Find("sale").GetComponent<TextMeshProUGUI>().text = reader_ind.GetInt32(0).ToString();
            }

        }

        dbManager.CloseConnection();
    }

}
