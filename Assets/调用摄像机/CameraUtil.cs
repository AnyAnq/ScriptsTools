using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraUtil
{
    /// <summary>
    /// 开启摄像机
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenCamera(Action OnComplete = null)
    {
        yield return Application.HasUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            /*打开摄像头*/
            var camDevice = WebCamTexture.devices;
            if (camDevice.Length > 0)
            {
                WebCamTexture webCamTexture = new WebCamTexture(camDevice[0].name, Screen.width, Screen.height);
                webCamTexture.Play();
            }
            else
            {
                Debug.LogWarning("请检查摄像头!");
            }
        }
    }


    /// <summary>
    /// 截取不同相机的画面
    /// </summary>
    /// <param name="_camera"></param>
    /// <param name="path"></param>
    /// <param name="OnProgress"></param>
    /// <returns></returns>
    private IEnumerator ScreenShoot(Camera _camera, string path, Action<RenderTexture> OnProgress = null)
    {
        //等待所有的摄像机和GUI被渲染完成。
        yield return new WaitForEndOfFrame();
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        _camera.targetTexture = rt;
        _camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------   
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        if (OnProgress != null)
        {
            OnProgress.Invoke(rt);
        } //  将截图显示到UI面板上

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //new Rect(0, 0, screenShot.width, screenShot.height) 可替换 Screen.safeArea
        screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0,
            0); // 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();
        // 重置相关参数，以使用camera继续在屏幕上显示  
        _camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        // GameObject.Destroy(rt);        // 没有销毁一直在内存中 
        // 最后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }


    /// <summary>
    /// 保存外部摄像机部分画面
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="index"></param>
    public void CaptureCameraPic(int width, int height, WebCamTexture camTex)
    {
        // 新建纹理
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        //获取纹理大小必须和新建纹理大小一模一样！  必须一模一样！
        Color[] colors = camTex.GetPixels(0, 0, tex.width, tex.height);
        tex.SetPixels(colors, 0);

        tex.Apply(false);
        byte[] imagebytes = tex.EncodeToPNG();
        if (!Directory.Exists("C:/")) Directory.CreateDirectory("C:/");

        File.WriteAllBytes("C:/1.png", imagebytes);
        Debug.Log("储存完成");
    }
}