using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonOnSound, buttonOffSound, notificationSound, moneySound;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playSound(string type)
    {
        if (type == "notification")
        {
            audioSource.PlayOneShot(notificationSound);
        }else if(type == "buttonOn")
        {
            Debug.Log("button on played");
            audioSource.PlayOneShot(buttonOnSound);
        }
        else if (type == "buttonOff")
        {
            audioSource.PlayOneShot(buttonOffSound);
        }
        else if (type == "money")
        {
            audioSource.PlayOneShot(moneySound);
        }
    }
}
