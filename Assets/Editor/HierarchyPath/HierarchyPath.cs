/*-------------------
 * 作者:侒
 * 时间:2021年01月28日 星期四 14:38
 * 功能:选中物体路径
 -------------------*/

using System;
using UnityEditor;
using UnityEngine;

public class HierarchyPath : MonoBehaviour
{
    private static readonly TextEditor CopyTool = new TextEditor();
    
    /// <summary>
    /// 复制GameObject的名字到剪贴板
    /// </summary>
    [MenuItem("GameObject/Copy Tool/Copy Name &C", priority = 0)]
    static void CopyTransName()
    {
        Transform trans = Selection.activeTransform;
        if (trans == null) return;
        CopyTool.text = trans.name;
        CopyTool.SelectAll();
        CopyTool.Copy();
    }

    /// <summary>
    /// 讲一个GameObject在Hierarchy中的完整路径拷贝到剪切板
    /// %  Ctrl
    /// #  Shift
    /// &  Alt
    /// </summary>
    [MenuItem("GameObject/Copy Tool/Copy Path", priority = 1)]
    static void CopyTransPath()
    {
        Transform trans = Selection.activeTransform; //选择的物体
        if (trans == null) return;
        CopyTool.text = GetTransPath(trans);
        CopyTool.SelectAll();
        CopyTool.Copy();
    }

    /// <summary>
    /// 获得GameObject在Hierarchy中的完整路径
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    private static string GetTransPath(Transform trans)
    {
        if (trans == null) return String.Empty;
        if (trans.parent == null) return trans.name;
        return GetTransPath(trans.parent) + "/" + trans.name;
    }
}