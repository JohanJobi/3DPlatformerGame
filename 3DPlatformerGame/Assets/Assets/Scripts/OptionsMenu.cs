using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    Resolution[] resolutions;//array of available resolutions
    public TMP_Dropdown resolutionDropdown; //makes it a variable you can put in to the inspector
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); //clears current options on dropdown

        List<string> options = new List<string>();//list is an array with a changable size in this case it contains the options
        int currentResolutionIndex = 0;
        for (int i=0; i<resolutions.Length; i++)//loop through each element and makes a userfriendly string to display on the drop down
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option); // adds to option list for every i

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {//checks if current resolution is equal to current resolution of display so it displays automatically on drop down
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options); // adds options list with every avaiable resolution and displays it
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }   

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);

    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
