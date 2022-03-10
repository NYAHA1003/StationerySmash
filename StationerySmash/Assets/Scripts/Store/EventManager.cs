using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private Dictionary<string, Action> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("씬안에 이벤트매니저 없음");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action>();
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //기존 이벤트에 더 많은 이벤트 추가 
            thisEvent += listener;

            //딕셔너리 업데이트
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //처음으로 사전에 이벤트 추가 
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (eventManager == null) return;
        Action thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //기존 이벤트에서 이벤트 제거
            thisEvent -= listener;

            //이벤트 업데이트 
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
            //OR USE instance.eventDictionary[eventName]();
        }
    }
}