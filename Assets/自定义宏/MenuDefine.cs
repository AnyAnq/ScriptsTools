using UnityEditor;
using UnityEngine;

/// <summary>
/// 自定义菜单类.
/// </summary>
public class Menu
{
    [MenuItem("Tools/Setting")]
    public static void Settings()
    {
        SettingDefine sw = EditorWindow.GetWindow<SettingDefine>();//获取指定类型的窗口.
        sw.titleContent = new GUIContent("设置窗口");
        sw.Show();
    }
}