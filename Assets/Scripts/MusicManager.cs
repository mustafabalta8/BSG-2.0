using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonOnSound, buttonOffSound, notificationSound, moneySound, newDaySound;
    [SerializeField] AudioSource audioSource;

    public AudioMixer audioMixer;
    void Awake()
    {
        SetUpSingelton();
        audioSource = GetComponent<AudioSource>();
    }
    private void SetUpSingelton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        
        audioSource.volume = 0.8f;  //not working
    }
    public void SetVolume(float volume)
    {
        //audioMixer.SetFloat("MyExposedParam",volume);
        audioSource.volume = volume;
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
        else if (type == "newDay")
        {
            audioSource.PlayOneShot(newDaySound);
        }
    }
}
