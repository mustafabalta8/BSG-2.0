using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Obsolete]

public class UIManager : MonoBehaviour
{
    //just one serialize field is enough

    [SerializeField] GameObject panel;

    public void OpenPanel()
    {
        if (!gameObject.active){
            gameObject.SetActive(true);
        }else{
            gameObject.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}