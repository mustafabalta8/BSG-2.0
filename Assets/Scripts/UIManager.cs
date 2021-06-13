using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject panel;
    
    MusicManager soundManager;

    public void OpenPanel()
    {
        soundManager = FindObjectOfType<MusicManager>();

        if (!gameObject.activeSelf){
            soundManager.playSound("buttonOn");
            gameObject.SetActive(true);
        }else{
            soundManager.playSound("buttonOff");
            gameObject.SetActive(false);
        }
    }
}