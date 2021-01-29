using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private BoxCollider box;
    void Start()
    {
        DebugTrace.Instance.SetLogOptions(true,2,editorCreate:true);//设置日志打开 显示2帧 并且编辑器下产生日志
        DebugTrace.Instance.StartTrace();
        Debug.Log("log");
        Debug.Log("log", this);
        Debug.LogError("logError");
        Debug.LogAssertion("LogAssertion");

        box.enabled = false;	//变量没有赋值是空的  所有log自动打印
    }

}
