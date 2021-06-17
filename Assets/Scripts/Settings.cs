using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Dropdown resDropdown;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;

        DefineResolutions();

       // SetScreenSize(3);
    }
    void DefineResolutions()
    {
        resolutionDropdown.ClearOptions();

        // resDropdown.ClearOptions();  
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == 60)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
                // if (i >= 8)
                // {
                options.Add(option);
                // }

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    Debug.Log(Screen.currentResolution);
                    Debug.Log(resolutions[i].width + "x" + resolutions[i].height);
                    currentResolutionIndex = i;
                    Debug.Log("currentResolutionIndex:" + currentResolutionIndex);
                }
                resolutionDropdown.AddOptions(options);
                // resDropdown.AddOptions(options);

               

                // resDropdown.value = currentResolutionIndex;
                // resDropdown.RefreshShownValue();
            }
        }
        resolutionDropdown.value = 21;
        //resolutionDropdown.Select();
        resolutionDropdown.RefreshShownValue();
        Debug.Log("resolutions.Length:" + resolutions.Length);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetResolution(int ResolutionIndex)
    {
        Debug.Log("ResolutionIndex" + ResolutionIndex);
        Resolution resolution = resolutions[ResolutionIndex];//-8
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log(resolution.width + "x" + resolution.height);
        

    }

    List<int> widths = new List<int>() { 960, 1280,1366, 1920 };
    List<int> heights = new List<int>() { 540, 800,768, 1080 };

    public void SetScreenSize(int index)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, fullscreen);
    }

}
