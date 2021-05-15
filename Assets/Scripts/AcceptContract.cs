using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptContract : MonoBehaviour
{
    public string platform;
    public string sofType;
    public string companyName;
    public int duration;
    public int offer;
    public int code;
    public int art;
    public int design;

    Company company;

    // Start is called before the first frame update
    void Start()
    {
        company = FindObjectOfType<Company>();
    }
    public void AssignEmployee()
    {
        
    }
    
}
