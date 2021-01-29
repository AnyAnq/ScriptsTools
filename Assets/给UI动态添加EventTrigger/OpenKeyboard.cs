using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenKeyboard : MonoBehaviour
{
    public InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        EventTriggerUtil.BindEventTrigger(EventTypeEnum.OnSelect, OnInputSelect,
            inputField.transform);
    }

    void OnInputSelect(BaseEventData data)
    {
        Process.Start(@"C:\Windows\System32\osk.exe");
    }
}

public class EventTriggerUtil
{
    /// <summary>
    /// 为UI绑定EventTrigger事件  可自行扩展
    /// </summary>
    /// <param name="type"></param>
    /// <param name="uiEvent"></param>
    /// <param name="root"></param>
    public static void BindEventTrigger(EventTypeEnum type, UnityAction<BaseEventData> uiEvent, Transform root)
    {
        if (!root.GetComponent<EventTrigger>())
        {
            root.gameObject.AddComponent<EventTrigger>();
        }

        UnityAction<BaseEventData> selectEvent = new UnityAction<BaseEventData>(uiEvent);
        EventTrigger.Entry onSelect = new EventTrigger.Entry();

        switch (type)
        {
            case EventTypeEnum.OnSelect:
                onSelect.eventID = EventTriggerType.Select;
                break;
            case EventTypeEnum.Deselect:
                onSelect.eventID = EventTriggerType.Deselect;
                break;
        }

        onSelect.callback.AddListener(selectEvent);
        root.GetComponent<EventTrigger>().triggers.Add(onSelect);
    }
}

public enum EventTypeEnum
{
    OnSelect,
    Deselect
}