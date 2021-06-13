using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //just one serialize field is enough

    [SerializeField] GameObject panel;

    public void OpenPanel()
    {
        if (!gameObject.activeSelf){
            gameObject.SetActive(true);
        }else{
            gameObject.SetActive(false);
        }
    }
}