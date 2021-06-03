using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class Product : MonoBehaviour
{

    public int productId { get; set; }

    public string companyName { get; set; }

    public string productName { get; set; }
    public string softwareType { get; set; }
    public string platform { get; set; }

    public int code_dif { get; set; }
    public int art_dif { get; set; }
    public int des_dif { get; set; }

    public int timeStared { get; set; }
    public int timeFinished { get; set; }

    public int quality { get; set; }

    public int amountSold { get; set; }
    public int price { get; set; }

    public Product()
    {

    }

    public Product(int product_id)
    {
        DbManager dbManager = FindObjectOfType<DbManager>();

        string query = string.Format("SELECT * FROM products WHERE id = {0}", product_id);

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            try
            {
                this.productId = product_id;
                this.productName = reader.GetString(1);
                this.softwareType = reader.GetString(2);
                this.platform = reader.GetString(3);
                this.timeStared = reader.GetInt32(4);
                this.timeFinished = reader.GetInt32(5);
                this.code_dif = reader.GetInt32(6);
                this.art_dif = reader.GetInt32(7);
                this.des_dif = reader.GetInt32(8);
                this.quality = reader.GetInt32(9);
                this.companyName = reader.GetString(10);
                this.amountSold = 0;
                this.price = reader.GetInt32(11);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        string query_ind = string.Format($"SELECT amountSold FROM products_sold WHERE productId = {productId}");

        IDataReader reader_ind = dbManager.ReadRecords(query_ind);

        while (reader_ind.Read())
        {
            this.amountSold = reader_ind.GetInt32(0);
        }
        dbManager.CloseConnection();
    }

    public int CreateProduct(DbManager dbManager)
    {
        string query = string.Format("INSERT INTO products VALUES (null,\"{0}\",\"{1}\",\"{2}\",{3},{4},{5},{6},{7},{8},\"{9}\",{10})", productName, platform, softwareType, timeStared, timeFinished, code_dif, art_dif, des_dif, quality, companyName,calculatePrice());
        dbManager.InsertRecords(query);

        query = "SELECT * FROM products ORDER BY id DESC LIMIT 1";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int productId = reader.GetInt32(0);
            return productId;
        }

        return 0;
    }

    public int calculateQuality()
    {
        quality = (int)(code_dif * 0.2) + (int)(art_dif * 0.15) + (int)(des_dif * 0.15);
        return quality;
    }

    public bool startCreation()
    {
        return true;
    }

    public bool endCreation()
    {
        return false;
    }

    public void updateProduct(int quality_enhancement, int est_time)
    {
        code_dif = (int)(code_dif * ((quality_enhancement * 1) + (est_time*0.5)) / 2);
        art_dif = (int)(art_dif * ((quality_enhancement * 1) + (est_time * 0.5)) / 2);
        des_dif = (int)(des_dif * ((quality_enhancement * 1) + (est_time * 0.5)) / 2);

        quality = calculateQuality();
        price = calculatePrice();

        DbManager dbManager = FindObjectOfType<DbManager>();
        string query = string.Format("UPDATE products SET code_dif = {0}, art_dif = {1}, des_dif = {2}, quality = {3}, price={4} WHERE id = {5}",code_dif, art_dif, des_dif, quality, price, this.productId);
        dbManager.InsertRecords(query);
        dbManager.CloseConnection();
    }

    public int getSellAmount()
    {
        double price_multiplier;

        if (price >= 5 && price <= 10)
        {
            price_multiplier = 1.3;
        }else if(price >= 11 && price <= 15)
        {
            price_multiplier = 1.0;
        }
        else
        {
            price_multiplier = 0.8;
        }

        double sellAmount = quality * UnityEngine.Random.Range((quality*12), (quality * 50)) * price_multiplier;

        return (int)sellAmount;
    }

    public int calculatePrice()
    {
        int price;
        if (quality <= 4)
        {
            price = quality * UnityEngine.Random.Range(5, 12);
        }else if (quality <= 8)
        {
            price = quality * UnityEngine.Random.Range(12, 30);
        }
        else
        {
            price = quality * UnityEngine.Random.Range(30, 50);
        }
        return price;
    }
}
