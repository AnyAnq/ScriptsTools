using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenSetting : MonoBehaviour
{
    void Awake()
    {
        Resolution[] resolutions = Screen.resolutions;
        int width = resolutions[0].width;
		int height = resolutions[0].height;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width > width)
            {
                width = resolutions[i].width;
                height = resolutions[i].height;
            }
            if (resolutions[i].width == width && height > resolutions[i].height)
            {
                width = resolutions[i].width;
                height = resolutions[i].height;
            }
        }
        Screen.SetResolution(width, height, true);
    }
}
