/*-------------------
 * 作者:侒
 * 时间:2020年11月12日 星期四 22:34
 * 功能:旋转图片并且保存
 -------------------*/

using System;
using System.Drawing;
using System.Drawing.Imaging;

public static class RevolvePhotoSaveUtil
{  
    /// <summary>
    /// 图片旋转指定角度API
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Image RotateImg(string filename, int angle)
    {
        Image img = Image.FromFile(filename);
        return RotateImg(img, angle);
    }

    /// <summary>
    /// 旋转指定角度
    /// </summary>
    /// <param name="b"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    static Image RotateImg(Image b, int angle)
    {
        angle = angle % 360;
        
        //弧度转换

        double radian = angle * Math.PI / 180.0;

        double cos = Math.Cos(radian);

        double sin = Math.Sin(radian);


        //原图的宽和高

        int w = b.Width;

        int h = b.Height;

        int W = (int) (Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));

        int H = (int) (Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));


        //目标位图

        Bitmap dsImage = new Bitmap(W, H);

        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);

        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


        //计算偏移量

        Point Offset = new Point((W - w) / 2, (H - h) / 2);


        //构造图像显示区域：让图像的中心与窗口的中心点一致

        Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);

        Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);


        g.TranslateTransform(center.X, center.Y);

        g.RotateTransform(360 - angle);


        //恢复图像在水平和垂直方向的平移

        g.TranslateTransform(-center.X, -center.Y);

        g.DrawImage(b, rect);


        //重至绘图的所有变换

        g.ResetTransform();


        g.Save();

        g.Dispose();

        //保存旋转后的图片

        b.Dispose();

        //可返回出去再保存
        dsImage.Save("FocusPoint.jpg", ImageFormat.Png);

        return dsImage;
    }

}