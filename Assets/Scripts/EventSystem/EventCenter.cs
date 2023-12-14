using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    static Dictionary<FunctionType, Delegate> eventTable = new Dictionary<FunctionType, Delegate>();
    private static void OnListenerAdding(FunctionType functionType, Delegate action)
    {
        if (!eventTable.ContainsKey(functionType))
        {
            eventTable.Add(functionType, null);
        }
        Delegate d = eventTable[functionType];
        if (d != null && d.GetType() != action.GetType())
        {
            throw new Exception(string.Format("����Ϊ�¼�{0}��Ӳ�ͬ���͵�ί�С�{1}��{2}", functionType, d.GetType(), action.GetType()));
        }
    }
    private static void OnListenerRemoving(FunctionType functionType, Delegate action)
    {
        if (eventTable.ContainsKey(functionType))
        {
            Delegate d = eventTable[functionType];
            if (d == null)
            {
                throw new Exception(string.Format("�¼�{0}û�ж�Ӧ��ί��", functionType));
            }
            else if (d.GetType() != action.GetType())
            {
                throw new Exception(string.Format("����Ϊ�¼�{0}�Ƴ���ͬ���͵�ί�С�{1}��{2}", functionType, d.GetType(), action.GetType()));
            }

        }
        else
        {
            throw new Exception(string.Format("�¼���{0}�������ֵ���", functionType));
        }
    }
    private static void OnListenerRemoved(FunctionType functionType)
    {
        if (eventTable[functionType] == null)
        {
            eventTable.Remove(functionType);
        }
    }

    public static void AddListener(FunctionType functionType, Action action)
    {
        OnListenerAdding(functionType, action);
        eventTable[functionType] = (Action)eventTable[functionType] + action;
    }
    public static void AddListener<T>(FunctionType functionType, Action<T> action)
    {
        OnListenerAdding(functionType, action);
        eventTable[functionType] = (Action<T>)eventTable[functionType] + action;
    }
    public static void AddListener<T0, T1>(FunctionType functionType, Action<T0, T1> action)
    {
        OnListenerAdding(functionType, action);
        eventTable[functionType] = (Action<T0, T1>)eventTable[functionType] + action;
    }
    public static void AddListener<T0, T1, T2>(FunctionType functionType, Action<T0, T1, T2> action)
    {
        OnListenerAdding(functionType, action);
        eventTable[functionType] = (Action<T0, T1, T2>)eventTable[functionType] + action;
    }
    public static void RemoveListener(FunctionType functionType, Action action)
    {
        OnListenerRemoving(functionType, action);
        eventTable[functionType] = (Action)eventTable[functionType] - action;
        OnListenerRemoved(functionType);
    }
    public static void RemoveListener<T>(FunctionType functionType, Action<T> action)
    {
        OnListenerRemoving(functionType, action);
        eventTable[functionType] = (Action<T>)eventTable[functionType] - action;
        OnListenerRemoved(functionType);
    }
    public static void RemoveListener<T0, T1>(FunctionType functionType, Action<T0, T1> action)
    {
        OnListenerRemoving(functionType, action);
        eventTable[functionType] = (Action<T0, T1>)eventTable[functionType] - action;
        OnListenerRemoved(functionType);
    }
    public static void RemoveListener<T0, T1, T2>(FunctionType functionType, Action<T0, T1, T2> action)
    {
        OnListenerRemoving(functionType, action);
        eventTable[functionType] = (Action<T0, T1, T2>)eventTable[functionType] - action;
        OnListenerRemoved(functionType);
    }
    public static void Broadcast(FunctionType functionType)
    {
        Delegate d;
        if (eventTable.TryGetValue(functionType, out d))
        {
            Action action = (Action)d;
            if (action != null)
            {
                action();
            }
            else
            {
                throw new Exception(string.Format("�㲥���¼�Ƶ��������{0}", functionType));
            }
        }
    }
    public static void Broadcast<T>(FunctionType functionType, T arg)
    {
        Delegate d;
        if (eventTable.TryGetValue(functionType, out d))
        {
            Action<T> action = (Action<T>)d;
            if (action != null)
            {
                action(arg);
            }
            else
            {
                throw new Exception(string.Format("�㲥���¼�Ƶ��������{0}", functionType));
            }
        }
    }
    public static void Broadcast<T0, T1>(FunctionType functionType, T0 arg0, T1 arg1)
    {
        Delegate d;
        if (eventTable.TryGetValue(functionType, out d))
        {
            Action<T0, T1> action = (Action<T0, T1>)d;
            if (action != null)
            {
                action(arg0, arg1);
            }
            else
            {
                throw new Exception(string.Format("�㲥���¼�Ƶ��������{0}", functionType));
            }
        }
    }
    public static void Broadcast<T0, T1, T2>(FunctionType functionType, T0 arg0, T1 arg1, T2 arg2)
    {
        Delegate d;
        if (eventTable.TryGetValue(functionType, out d))
        {
            Action<T0, T1, T2> action = (Action<T0, T1, T2>)d;
            if (action != null)
            {
                action(arg0, arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("�㲥���¼�Ƶ��������{0}", functionType));
            }
        }
    }
}
