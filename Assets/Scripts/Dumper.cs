using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumper : MonoBehaviour
{

    DbManager dbManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hey");
        dbManager = FindObjectOfType<DbManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
