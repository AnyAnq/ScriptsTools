/*-------------------
 * 作者:侒
 * 时间:2020年11月11日 星期三 18:33
 * 功能:鼠标控制 物体围绕目标点旋转 限定角度
 -------------------*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoundTargetRotAngle : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    private bool isPlay;
    private bool isClick;
    private float speed;

    private float temSpeed;

    public Transform target;
    public Transform cam;
    

    private void Update()
    {
		//屏幕触发
        if (Input.GetMouseButton(0))
        {
            isClick = true;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
        }

        speed = Input.GetAxis("Mouse X") * Time.deltaTime * 500;
        
        if(!isClick) return;
        cam.RotateAround(target.position,Vector3.up,speed);
    }


    //UI触发
    public void OnPointerDown(PointerEventData eventData)
    {
        isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
    }
}