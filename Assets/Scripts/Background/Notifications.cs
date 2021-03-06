using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
    [SerializeField] public int notificationDuration = 5;
    [SerializeField] GameObject notificatonContainer;
    [SerializeField] Image notificationImage;
    [SerializeField] TMP_Text notification_text;

    TimeManager timeManager;
    MusicManager soundManager;

    void Start()
    { 
        notificatonContainer.SetActive(false);

        timeManager = FindObjectOfType<TimeManager>();
        soundManager = FindObjectOfType<MusicManager>();


        notificationImage.canvasRenderer.SetAlpha(0.0f);
    }

    public void pushNotification(string notificationText)
    {
        notificatonContainer.SetActive(true);

        notification_text.text = notificationText;

        fadeIn();

        StartCoroutine(fadeOut());

        Debug.Log(notificationText);
    }

    public void fadeIn()
    {
        soundManager.playSound("notification");
        notificationImage.CrossFadeAlpha(1, 0.7f, false);
        StartCoroutine(FadeTextToFullAlpha(0.7f, notification_text));
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(notificationDuration*timeManager.timeScale);
        int fadeOutTime = 1;
        notificationImage.CrossFadeAlpha(0, fadeOutTime, false);
        StartCoroutine(FadeTextToZeroAlpha(fadeOutTime, notification_text));
    }

    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

        notificatonContainer.SetActive(false);
    }

}
