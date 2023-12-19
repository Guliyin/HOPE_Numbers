using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is not used in the current animator system.
/// But you can listen to changes of a list using this. It is like the OnValueChange event.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventList<T> : List<T>
{
    public Action<T> DOnAdd;
    public Action<T> DOnRemove;

    public new void Add(T item)
    {
        base.Add(item);
        DOnAdd?.Invoke(item);
    }
    public new void Remove(T item)
    {
        base.Remove(item);
        DOnRemove?.Invoke(item);
    }
}
