using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;

public class ProductManager : MonoBehaviour
{
    Product product = new Product();

    public string productName;
    public string softwareType;
    public string platform;


    public int code_dif = 0;
    public int art_dif = 0;
    public int des_dif = 0;

    public int estimated_time = 0;

    public int quality = 0;

    private bool initialization = true;
    private bool creationStarted = false;
    private int startedTime;

    public int productId = 0;
    int quality_enhancement = 0;
    private bool updateStarted = false;

    //creation gui
    [SerializeField] TextMeshProUGUI softwareTypeField;
    [SerializeField] TextMeshProUGUI platformField;


    [SerializeField] Text req_coding_skill;
    [SerializeField] Text req_art_skill;
    [SerializeField] Text req_des_skill;
    [SerializeField] Text est_time;

    [SerializeField] TextMeshProUGUI buttonText;


    [SerializeField] TMP_InputField productNameField;
    [SerializeField] TMP_Dropdown typeDropdown;
    [SerializeField] TMP_Dropdown platformDropdown;
    [SerializeField] Button startButton;
    //creation gui 


    //enhancement gui
    [SerializeField] TMP_Dropdown productDropDown;
    [SerializeField] Text fix_bugs_et;
    [SerializeField] Text add_new_feature_et;
    [SerializeField] Text port_to_new_os_et;

    [SerializeField] Button fix_bugs_start;
    [SerializeField] Button add_new_feature_start;
    [SerializeField] Button port_to_new_os_start;


    //enhancement gui

    DbManager dbManager;
    Company Company;
    TimeManager timeManager;
    AssignEmp assignEmp;
    Notifications notifications;

    [SerializeField] GameObject placeHolder;

    public void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        Company = FindObjectOfType<Company>();
        timeManager = FindObjectOfType<TimeManager>();
        assignEmp = FindObjectOfType<AssignEmp>();
        notifications = FindObjectOfType<Notifications>();

        req_coding_skill.text = "0";
        req_art_skill.text = "0";
        req_des_skill.text = "0";
        est_time.text = "0 weeks";

        startButton.interactable = false;


        getTypes();

        typeDropdown.onValueChanged.AddListener(delegate {
            string selectedValue = typeDropdown.options[typeDropdown.value].text;

            code_dif = 0;
            art_dif = 0;
            des_dif = 0;

            code_dif += getDifficulty("type", selectedValue)["code_dif"];
            art_dif += getDifficulty("type", selectedValue)["art_dif"];
            des_dif += getDifficulty("type", selectedValue)["des_dif"];

            getPlatforms();

            showRequiredSkills();

            checkButton();
        });

        platformDropdown.onValueChanged.AddListener(delegate {
            string selectedValue = platformDropdown.options[platformDropdown.value].text;

            code_dif += getDifficulty("platform", selectedValue)["code_dif"];
            art_dif += getDifficulty("platform", selectedValue)["art_dif"];
            des_dif += getDifficulty("platform", selectedValue)["des_dif"];

            assignEmp.code = code_dif;
            assignEmp.art = art_dif;
            assignEmp.design = des_dif;
            assignEmp.ProdID = GetID();

            showRequiredSkills();

            checkButton();
        });

        productNameField.onValueChanged.AddListener(delegate
        {
            checkButton();
        });


        getProducts();


        productDropDown.onValueChanged.AddListener(delegate {
            placeHolder.SetActive(false);
        });

    }
    int GetID()
    {
        int id = 0;
        string query = "SELECT * FROM products";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            id = reader.GetInt32(0);

        }
        dbManager.CloseConnection();

        return id + 1;
    }
    private void Update()
    {
        if (creationStarted || updateStarted)
        {
            checkIfFinished(product);
        }

    }
    public void createProduct()
    {
        Product product = new Product();

        startedTime = timeManager.displayTime;

        softwareType = typeDropdown.options[typeDropdown.value].text;
        platform = platformDropdown.options[platformDropdown.value].text;
        productName = productNameField.text;

        product.companyName = Company.companyName;

        product.productName = productName;
        product.softwareType = softwareType;
        product.platform = platform;

        product.code_dif = code_dif;
        product.art_dif = art_dif;
        product.des_dif = des_dif;

        product.calculateQuality();

        product.timeStared = timeManager.displayTime;

        creationStarted = product.startCreation();

        notifications.pushNotification($"Development for {productName} is started. It will be completed on {(timeManager.displayTime + estimated_time)}");
        this.product = product;
    }

    public void checkIfFinished(Product product)
    {
        if ((startedTime + estimated_time) <= timeManager.displayTime)
        {
            if (!updateStarted)
            {
                creationStarted = product.endCreation();
                product.timeFinished = timeManager.displayTime;
                product.CreateProduct(dbManager);

                notifications.pushNotification($"{productName} is completed for {platform} platform, duration {estimated_time} weeks");
            }
            else
            {
                notifications.pushNotification($"Enhancement for {product.productName} is completed.");
                product.updateProduct(quality_enhancement, estimated_time);
                updateStarted = false;
            }
        }
    }

    public void getTypes()
    {
        dbManager = FindObjectOfType<DbManager>();
        string query = "SELECT * FROM software_types";
        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string type = reader.GetString(1);

            typeDropdown.options.Add(new TMP_Dropdown.OptionData() { text = type });

        }
        dbManager.CloseConnection();
        platformDropdown.interactable = false;
    }

    public void getPlatforms()
    {
        if (initialization)
        {
            dbManager = FindObjectOfType<DbManager>();
            string query = "SELECT * FROM platforms";
            IDataReader reader = dbManager.ReadRecords(query);

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string type = reader.GetString(1);

                platformDropdown.options.Add(new TMP_Dropdown.OptionData() { text = type });

            }
            dbManager.CloseConnection();

            platformDropdown.interactable = true;
            initialization = false;
        }
        else
        {
            return;
        }

    }

    public void getProducts()
    {
        if (initialization)
        {
            dbManager = FindObjectOfType<DbManager>();
            string query = string.Format("SELECT * FROM products WHERE company = \"{0}\"",Company.companyName);
            IDataReader reader = dbManager.ReadRecords(query);

            while (reader.Read())
            {
                string product_name = reader.GetString(1);

                productDropDown.options.Add(new TMP_Dropdown.OptionData() { text = product_name });

            }
            dbManager.CloseConnection();
        }
        else
        {
            return;
        }
    }

    public IDictionary<string, int> getDifficulty(string type, string name)
    {
        dbManager = FindObjectOfType<DbManager>();

        IDictionary<string, int> difficulties = new Dictionary<string, int>();

        string query = string.Format("SELECT code_dif, art_dif, des_dif FROM software_types WHERE type = \"{0}\"", name);

        if (type == "platform")
        {
            query = string.Format("SELECT code_dif, art_dif, des_dif FROM platforms WHERE platform = \"{0}\"", name);
        }

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            int code_dif = reader.GetInt32(0);
            int art_dif = reader.GetInt32(1);
            int des_dif = reader.GetInt32(2);

            difficulties.Add("code_dif", code_dif);
            difficulties.Add("art_dif", art_dif);
            difficulties.Add("des_dif", des_dif);
        }
        dbManager.CloseConnection();

        return difficulties;
    }

    public void showRequiredSkills()
    {
            req_coding_skill.text = string.Format("{0}", code_dif);
            req_art_skill.text = string.Format("{0}", art_dif);
            req_des_skill.text = string.Format("{0}", des_dif);

            estimated_time = (code_dif + art_dif + des_dif) / 10;

            est_time.text = string.Format("{0} weeks", estimated_time);

        assignEmp.duration = estimated_time;
    }

    public void checkButton()
    {
        if (typeDropdown.options[typeDropdown.value].text != "Select a type" && platformDropdown.options[platformDropdown.value].text != "Select a platform" && productNameField.text != "")
        {
            buttonText.text = "Assign Employees";
            startButton.interactable = true;
        }
        else
        {
            if(productNameField.text == "")
            {
                buttonText.text = "Select a name";
                startButton.interactable = false;
            }
            if (typeDropdown.options[typeDropdown.value].text == "Select a type")
            {
                buttonText.text = "Select a type";
                startButton.interactable = false;
            }
            if (platformDropdown.options[platformDropdown.value].text == "Select a platform")
            {
                buttonText.text = "Select a platform";
                startButton.interactable = false;
            }
        }
    }

    public void getEnhancementDetails(string enhancement)
    {
        dbManager = FindObjectOfType<DbManager>();

        string selectedProductName = productDropDown.options[productDropDown.value].text;
        string enhancementType = "";

        string query = string.Format("SELECT id FROM products WHERE productName = \"{0}\"", selectedProductName);

        IDataReader reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            productId = reader.GetInt32(0);
        }
        dbManager.CloseConnection();

        Product selectedProduct = new Product(productId);


        query = string.Format("SELECT * FROM enhancements WHERE enhancement = \"{0}\"", enhancement);

        reader = dbManager.ReadRecords(query);

        while (reader.Read())
        {
            enhancementType = reader.GetString(1); 
            quality_enhancement = reader.GetInt32(3);
            estimated_time = reader.GetInt32(4);
        }
        dbManager.CloseConnection();

        if(enhancementType == "Add a new Feature")
        {
            add_new_feature_et.text = estimated_time.ToString();
        }
        else if(enhancementType == "Fix Bugs")
        {
            fix_bugs_et.text = estimated_time.ToString();
        }
        else
        {
            port_to_new_os_et.text = estimated_time.ToString();
        }
    }

    public void startEnhancement()
    {
        startedTime = timeManager.displayTime;

        string product = productDropDown.options[productDropDown.value].text;

        notifications.pushNotification($"Product enhancement STARTED for {product}, duration {estimated_time} weeks (Will be finished on {(timeManager.displayTime + estimated_time)})");
        updateStarted = true;
    }

}
