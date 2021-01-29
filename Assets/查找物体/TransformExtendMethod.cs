using System.Collections.Generic;
using UnityEngine;

public static class TransformExtendMethod
{
    #region 查找物体 <!-递归-!>

    /// <summary>
    ///     递归查找第一个对应名字物体   <返回Transform>
    /// </summary>
    /// <param name="root">从该根物体下找</param>
    /// <param name="targetName">查询目标名字</param>
    /// <returns></returns>
    public static Transform FindRecursively(this Transform root, string targetName)
    {
        if (root.name == targetName) return root;

        foreach (Transform child in root)
        {
            var t = child.FindRecursively(targetName);
            if (t != null) return t;
        }

        return null;
    }

    /// <summary>
    ///     递归查找第一个对应名字物体的组件  <返回Component>
    /// </summary>
    /// <param name="root">从该根物体下找</param>
    /// <param name="targetName">查询目标名字</param>
    /// <returns></returns>
    public static T FindComponent<T>(this Transform root, string targetName)
    {
        if (root.name == targetName) return root.GetComponent<T>();

        foreach (Transform child in root)
        {
            var t = child.FindRecursively(targetName);
            if (t != null) return t.GetComponent<T>();
        }

        return default;
    }

    /// <summary>
    ///     查找父物体下所有相同名字的物体    <see cref="List{Transform}"/>
    /// </summary>
    /// <param name="root">根物体</param>
    /// <param name="targetName">查询目标名字</param>
    /// <param name="temList">所有同名物体集合</param>
    public static void FindAllSameName(this Transform root, string targetName, ref List<Transform> temList)
    {
        if (root.name == targetName) temList.Add(root);

        foreach (Transform t in root) FindAllSameName(t, targetName, ref temList);
    }

    /// <summary>
    ///     查找root下所有拥有 <T>组件 的物体    <see cref="List{T}"/>
    /// </summary>
    /// <param name="root"></param>
    /// <param name="temList"></param>
    /// <typeparam name="T"></typeparam>
    public static void FindAllSameComponent<T>(this Transform root, ref List<T> temList)
    {
        var C = root.GetComponent<T>();
        if (C != null) temList.Add(C);

        foreach (Transform t in root) FindAllSameComponent(t, ref temList);
    }

    #endregion

    #region 属性设置

    /// <summary>
    ///     修改物体显示状态
    /// </summary>
    /// <param name="root"></param>
    /// <param name="value"></param>
    public static void SetActive(this Transform root, bool value)
    {
        root.gameObject.SetActive(value);
    }

    /// <summary>
    ///     修改localPosition.x
    /// </summary>
    /// <param name="root"></param>
    /// <param name="x"></param>
    public static void SetX(this Transform root, float x)
    {
        root.localPosition = new Vector3(x, root.localPosition.y, root.localPosition.z);
    }

    /// <summary>
    ///     修改localPosition.y
    /// </summary>
    /// <param name="root"></param>
    /// <param name="y"></param>
    public static void SetY(this Transform root, float y)
    {
        root.localPosition = new Vector3(root.localPosition.y, y, root.localPosition.z);
    }

    /// <summary>
    ///     修改localPosition.z
    /// </summary>
    /// <param name="root"></param>
    /// <param name="z"></param>
    public static void SetZ(this Transform root, float z)
    {
        root.localPosition = new Vector3(root.localPosition.x, root.localPosition.y, z);
    }

    #endregion
}