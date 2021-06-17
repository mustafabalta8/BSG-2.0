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
        resolutionDropdown.ClearOptions();

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height+" "+resolutions[i].refreshRate+"Hz";
           // if (i >= 8)
           // {
                options.Add(option);
           // }
            
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                Debug.Log(Screen.currentResolution);
                Debug.Log(resolutions[i].width+"x"+ resolutions[i].height);
                currentResolutionIndex = i;
                Debug.Log("currentResolutionIndex:"+currentResolutionIndex);
            }
            resolutionDropdown.AddOptions(options);
            resDropdown.AddOptions(options);

            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            resDropdown.value = currentResolutionIndex;
            resDropdown.RefreshShownValue();
        }
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

}
