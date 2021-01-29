using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DebugTrace
{
    private FileStream fileStream;
    private StreamWriter streamWriter;

    private bool isEditorCreate = false;  //是否在编辑器中也产生日志文件
    private int showFrames = 1000;  //打印所有

    #region instance

    private static readonly object obj = new  object();
    private static DebugTrace m_instance;

    public static DebugTrace Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (obj)
                {
                    if (m_instance == null)
                    {
                        m_instance = new DebugTrace();
                    }
                }
            }
            return m_instance;
        }
    }

    #endregion


    public void StartTrace()
    {
        if (Debug.unityLogger.logEnabled)
        {
            if (Application.isEditor)
            {
                //在编辑器中设置 isEditorCreate == true 时候产生日志
                if (isEditorCreate)
                {
                    CreateOutlog();
                }
            }
            //不在编辑器中  是否产生日志 Debug.unityLogger.logEnabled控制
            else
            {
                CreateOutlog();
            }
        }
    }

    private void CreateOutlog()
    {
        if (!Directory.Exists(Application.dataPath + "/../" + "Outlog"))
        {
            Directory.CreateDirectory(Application.dataPath + "/../" + "Outlog");
        }

        string path = Application.dataPath + "/../Outlog" + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_log.txt";
        fileStream = new FileStream(path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
        streamWriter = new StreamWriter(fileStream);
        Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;

    }

    private void Application_logMessageReceivedThreaded(string condition, string stacktrace, LogType type)
    {
        //Debug.Log(stacktrace);  //打包后stacktrace为空  所以要自己实现

        if (type != LogType.Warning)
        {
            // StackTrace stack = new StackTrace(1,true); //跳过第二?（1）帧
            StackTrace stack = new StackTrace(true);  //捕获所有帧
            string stackStr = String.Empty;

            int frameCount = stack.FrameCount; //帧数
            if (this.showFrames > frameCount)
            {
                this.showFrames = frameCount; //如果帧数大于总帧速 设置一下
            }

            //自定义输出帧数,可以自行试试查看效果
            for (int i = stack.FrameCount - this.showFrames; i < stack.FrameCount; i++)
            {
                StackFrame sf = stack.GetFrame(i);//获取当前帧信息

                // 1:第一种    ps:GetFileLineNumber 在发布打包后获取不到
                stackStr += "at [" + sf.GetMethod().DeclaringType.FullName +
                            "_" + sf.GetMethod().Name + "_Line: " + sf.GetFileLineNumber() + "]\n       ";
                //或者直接调用tostring 显示数据过多 且打包后有些数据获取不到
                // stackStr += sf.ToString();

            }
            //或者 stackStr = stack.ToString();
            string content = string.Format("time:{0}    logType:{1}   logString:{2} \nstackTrace:{3}{4}",
                DateTime.Now.ToString("HH:mm:ss"), type, condition, stacktrace, "\r\n");
            streamWriter.WriteLine(content);
            streamWriter.Flush();
        }
    }

    /// <summary>
    /// 设置选项
    /// </summary>
    /// <param name="logEnable">是否记录日志</param>
    /// <param name="showFrams">是否显示所有堆栈帧 默认只显示当前帧 如果设为0 则显示所有帧</param>
    /// <param name="filterLogType">过滤 默认log级别以上</param>
    /// <param name="editorCreate">是否在编辑器中产生日志记录 默认不需要</param>
    public void SetLogOptions(bool logEnable, int showFrams = 1, LogType filterLogType = LogType.Log,bool editorCreate = false)
    {
        Debug.unityLogger.logEnabled = logEnable;
        Debug.unityLogger.filterLogType = filterLogType;
        isEditorCreate = editorCreate;
        this.showFrames = showFrams == 0 ? 1000 : showFrams;
    }

    /// <summary>
    /// 关闭跟踪日志
    /// </summary>
    public void CloseTrace()
    {
        Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;
        streamWriter.Dispose();
        streamWriter.Close();
        fileStream.Dispose();
        fileStream.Close();
    }
}