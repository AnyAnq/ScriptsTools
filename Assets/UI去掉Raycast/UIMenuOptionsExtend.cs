// Add by yaukey(https://github.com/yaukeywang) at 2017-10-11.
// Extend default ui menu.

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;

[InitializeOnLoad]
static internal class UIMenuOptionsExtend
{
    // The reflected dafault methods.
    private static MethodInfo m_miGetDefaultResource = null;
    private static MethodInfo m_miPlaceUIElementRoot = null;

    static UIMenuOptionsExtend()
    {
        Initialize();
    }

    private static void Initialize()
    {
        // Get all loaded assemblies.
        Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly uiEditorAssembly = null;
        foreach (Assembly assembly in allAssemblies)
        {
            AssemblyName assemblyName = assembly.GetName();
            if ("UnityEditor.UI" == assemblyName.Name)
            {
                uiEditorAssembly = assembly;
                break;
            }
        }

        // Check if we find ui assembly.
        if (null == uiEditorAssembly)
        {
            Debug.LogError("Can not find assembly: UnityEditor.UI.dll");
            return;
        }

        // Get things we need.
        Type menuOptionType = uiEditorAssembly.GetType("UnityEditor.UI.MenuOptions");
        m_miGetDefaultResource =
            menuOptionType.GetMethod("GetStandardResources", BindingFlags.NonPublic | BindingFlags.Static);
        m_miPlaceUIElementRoot =
            menuOptionType.GetMethod("PlaceUIElementRoot", BindingFlags.NonPublic | BindingFlags.Static);
    }

    [MenuItem("GameObject/UI/Text", false, 2000)]
    static public void AddText(MenuCommand menuCommand)
    {
        GameObject go =
            DefaultControls.CreateText((DefaultControls.Resources) m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] {go, menuCommand});

        // Remove raycast target.
        Text text = go.GetComponent<Text>();
        text.raycastTarget = false;
    }

    [MenuItem("GameObject/UI/Image", false, 2001)]
    static public void AddImage(MenuCommand menuCommand)
    {
        GameObject go =
            DefaultControls.CreateImage((DefaultControls.Resources) m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] {go, menuCommand});

        // Remove raycast target.
        Image image = go.GetComponent<Image>();
        image.raycastTarget = false;
    }

    [MenuItem("GameObject/UI/Raw Image", false, 2002)]
    static public void AddRawImage(MenuCommand menuCommand)
    {
        GameObject go =
            DefaultControls.CreateRawImage((DefaultControls.Resources) m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] {go, menuCommand});

        // Remove raycast target.
        RawImage rawImage = go.GetComponent<RawImage>();
        rawImage.raycastTarget = false;
    }

    [MenuItem("GameObject/UI/Button", false, 2030)]
    static public void AddButton(MenuCommand menuCommand)
    {
        GameObject go =
            DefaultControls.CreateButton((DefaultControls.Resources) m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] {go, menuCommand});

        // Remove raycast target.
        Text text = go.transform.Find("Text").GetComponent<Text>();
        text.raycastTarget = false;
    }

    //[MenuItem("GameObject/UI/Toggle", false, 2031)]
    static public void AddToggle(MenuCommand menuCommand)
    {
    }

    //[MenuItem("GameObject/UI/Slider", false, 2033)]
    static public void AddSlider(MenuCommand menuCommand)
    {
    }

    //[MenuItem("GameObject/UI/Scrollbar", false, 2034)]
    static public void AddScrollbar(MenuCommand menuCommand)
    {
    }

    [MenuItem("GameObject/UI/Dropdown", false, 2035)]
    static public void AddDropdown(MenuCommand menuCommand)
    {
        GameObject go =
            DefaultControls.CreateDropdown((DefaultControls.Resources) m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] {go, menuCommand});

        // Remove raycast target.
        Text textLabel = go.transform.Find("Label").GetComponent<Text>();
        textLabel.raycastTarget = false;

        Text textItemLabel = go.transform.Find("Template/Viewport/Content/Item/Item Label").GetComponent<Text>();
        textItemLabel.raycastTarget = false;
    }

    [MenuItem("GameObject/UI/Input Field", false, 2036)]
    public static void AddInputField(MenuCommand menuCommand)
    {
        GameObject go =
            DefaultControls.CreateInputField((DefaultControls.Resources) m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] {go, menuCommand});

        // Remove raycast target.
        Text textPlaceholder = go.transform.Find("Placeholder").GetComponent<Text>();
        textPlaceholder.raycastTarget = false;

        Text textContent = go.transform.Find("Text").GetComponent<Text>();
        textContent.raycastTarget = false;
    }

    //[MenuItem("GameObject/UI/Canvas", false, 2060)]
    static public void AddCanvas(MenuCommand menuCommand)
    {
    }

    //[MenuItem("GameObject/UI/Panel", false, 2061)]
    static public void AddPanel(MenuCommand menuCommand)
    {
    }

    //[MenuItem("GameObject/UI/Scroll View", false, 2062)]
    static public void AddScrollView(MenuCommand menuCommand)
    {
    }
}