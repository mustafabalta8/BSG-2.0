using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewDay : MonoBehaviour
{
    [SerializeField] GameObject newDayContainer;
    [SerializeField] Image newDayBgImage;
    [SerializeField] TMP_Text newDayText;

    MusicManager soundManager;
    TimeManager timeManager;

    private void Start()
    {
        soundManager = FindObjectOfType<MusicManager>();
        timeManager = FindObjectOfType<TimeManager>();

        newDayContainer.SetActive(false);

        newDayBgImage.canvasRenderer.SetAlpha(0.0f);
    }

    public void newDay(string text)
    {
        newDayContainer.SetActive(true);

        newDayText.text = timeManager.calculateDate();

        fadeIn();

        StartCoroutine(fadeOut());
    }

    public void fadeIn()
    {
        if(timeManager.timeScale == 1)
        {
            soundManager.playSound("newDay");
        }

        newDayBgImage.CrossFadeAlpha(1, 0.7f, false);
        StartCoroutine(FadeTextToFullAlpha(0.7f, newDayText));
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(2);
        int fadeOutTime = 1;
        newDayBgImage.CrossFadeAlpha(0, fadeOutTime, false);
        StartCoroutine(FadeTextToZeroAlpha(fadeOutTime, newDayText));
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

        newDayContainer.SetActive(false);
    }
}
