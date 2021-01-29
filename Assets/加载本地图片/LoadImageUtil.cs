/*-------------------
 * 作者:侒
 * 时间:2020年11月06日 星期五 22:27
 * 功能:加载本地图片
 -------------------*/

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadImageUtil
{
    /// <summary>
    /// 使用www加载
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static IEnumerator Load(string url, Image image)
    {
        double startTime = (double) Time.time;
        //请求WWW
        WWW www = new WWW(url);

        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            //获取Texture
            Texture2D texture = www.texture;
            
            
            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
            

            startTime = (double) Time.time - startTime;
            Debug.Log("www加载用时 ： " + startTime);
        }
    }


    /// <summary>
    /// 以IO方式进行加载
    /// </summary>
    public static Sprite LoadByIo(string url)
    {
        double startTime = (double) Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int) fileStream.Length);

        //释放文件读取流
        fileStream.Close();
        //释放本机屏幕资源
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 300;
        int height = 372;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        // return texture;

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;

        startTime = (double) Time.time - startTime;
        Debug.Log("IO加载" + startTime);
    }
}