using UnityEngine;

public class CamTools
{
    public static float SmoothDampMaxSpeed = 10000f;
    public static float SmoothDampDeltaTime = 0.02f;
    public static float OverLookTargetMoveTime = 0.4f;
    public static float OverLookMoveTime = 0.5f;
}

public enum TargetType
{
    Exist,
    InExistecve
}

/* 依赖Editor/AlterInspector文件夹下的 CreateInspector.cs */

public class FreeCamera : MonoBehaviour
{
    private float distanceVelocity;
    private Vector3 lastMousePos = Vector3.zero;
    private Vector2 lastTP1 = Vector2.zero;
    private Vector2 lastTP2 = Vector2.zero;
    private Vector3 saveMousePos = Vector3.zero;
    private Vector3 smooth = Vector3.zero;
    private Vector3 velocityV3 = Vector3.zero;
    private float smoothDistance;
    private float offDis = 10;


    public TargetType targetType = TargetType.Exist;
    [Header("鼠标激活区域")] public Rect mouseActiveRect = new Rect(0, 0, 1, 1);
    [Header("缓冲时间")] [Range(0, 2)] public float smoothTime = 0.4f;
    [Header("相机操控的速度")] public Vector2 speed = Vector2.one;
    [Header("看向的目标点")] public Transform target;
    [Header("相机和目标点的缩放速度")] public float wheelSpeed = 25;
    [Header("相机与地平面的最大夹角")] public float yMaxLimit = 80;
    [Header("相机与地平面的最小夹角")] public float yMinLimit;
    [Header("相机和目标点之间的最大距离")] public float maxDistance = 100;
    [Header("相机和目标点之间的最小距离")] public float minDistance = 5;


    private void Start()
    {
        Initial();
    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Initial()
    {
        if (targetType == TargetType.Exist)
        {
            offDis = Vector3.Distance(target.position, transform.position);
            smoothDistance = offDis;
        }
        else
        {
            target = null;
        }

        lastMousePos = Input.mousePosition;
        saveMousePos = new Vector3(transform.eulerAngles.y, transform.eulerAngles.x, 0);
        saveMousePos.y = Mathf.Clamp(saveMousePos.y, yMinLimit, yMaxLimit);
        smooth = saveMousePos;
        mouseActiveRect = new Rect(Screen.width * mouseActiveRect.x, Screen.height * mouseActiveRect.y,
            Screen.width * mouseActiveRect.width, Screen.height * mouseActiveRect.height);
    }


    /// <summary>
    /// 位置更新
    /// </summary>
    private void Update()
    {
        MouseControl();

        offDis = Mathf.Clamp(offDis, minDistance, maxDistance);
        smoothDistance = Mathf.SmoothDamp(smoothDistance, offDis, ref distanceVelocity, smoothTime,
            CamTools.SmoothDampMaxSpeed, CamTools.SmoothDampDeltaTime);
        smooth = Vector3.SmoothDamp(smooth, saveMousePos, ref velocityV3, smoothTime, CamTools.SmoothDampMaxSpeed,
            CamTools.SmoothDampDeltaTime);
        transform.rotation = Quaternion.Euler(smooth.y, smooth.x, 0);

        if (target)
        {
            transform.position = transform.rotation * new Vector3(0, 0, -smoothDistance) + target.position;
        }
        else
        {
            var z = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * wheelSpeed * 10;
            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);
        }
    }

    /// <summary>
    /// 鼠标控制
    /// </summary>
    private void MouseControl()
    {
        if (!mouseActiveRect.Contains(Input.mousePosition))
        {
            lastMousePos = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0)) lastMousePos = Input.mousePosition;

            saveMousePos.x += (Input.mousePosition.x - lastMousePos.x) * speed.x;
            saveMousePos.y -= (Input.mousePosition.y - lastMousePos.y) * speed.y;
            lastMousePos = Input.mousePosition;
            saveMousePos.y = Mathf.Clamp(saveMousePos.y, yMinLimit, yMaxLimit);
        }

        offDis -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
    }


#if ISTOUCH
    private void TouchControl()
    {
        if (Input.touchCount == 2)
        {
            if (!mouseActiveRect.Contains(Input.GetTouch(0).position))
            {
                lastTP1 = Input.GetTouch(0).position;
                lastTP2 = Input.GetTouch(1).position;
                return;
            }

            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                lastTP1 = Input.GetTouch(0).position;
                lastTP2 = Input.GetTouch(1).position;
            }

            offDis -= (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) -
                       Vector2.Distance(lastTP1, lastTP2)) * wheelSpeed * 0.01f;
            lastTP1 = Input.GetTouch(0).position;
            lastTP2 = Input.GetTouch(1).position;
        }
        else
        {
            if (!mouseActiveRect.Contains(Input.mousePosition)) return;

            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0)) lastMousePos = Input.mousePosition;

                saveMousePos.x += (Input.mousePosition.x - lastMousePos.x) * speed.x;
                saveMousePos.y -= (Input.mousePosition.y - lastMousePos.y) * speed.y;
                lastMousePos = Input.mousePosition;
                saveMousePos.y = Mathf.Clamp(saveMousePos.y, yMinLimit, yMaxLimit);
            }
        }
    }
    
        private Vector2 V2SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime)
    {
        current.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime,
            CamTools.SmoothDampMaxSpeed, CamTools.SmoothDampDeltaTime);
        current.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime,
            CamTools.SmoothDampMaxSpeed, CamTools.SmoothDampDeltaTime);
        return current;
    }
#endif
}