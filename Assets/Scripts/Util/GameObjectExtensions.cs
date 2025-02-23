using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtensions
{
    public static List<MonoBehaviour> GetAllNestedScripts(this GameObject gameObject)
    {
        List<MonoBehaviour> scripts = new List<MonoBehaviour>();
        var newScripts = gameObject.GetComponents<MonoBehaviour>();
        if (newScripts != null && newScripts.Length != 0) scripts.AddRange(newScripts);
        var children = gameObject.GetComponentsInChildren<Transform>();
        if (children != null && children.Length != 0)
        {
            foreach (var transform in children)
            {
                if (transform.gameObject == gameObject) continue;
                scripts.AddRange(transform.gameObject.GetAllNestedScripts());
            }
        }
        return scripts;
    }

    public static List<T> GetAllNestedComponentsOfType<T>(this GameObject gameObject)
    {
        List<T> components = new List<T>();
        var newComps = gameObject.GetComponents<T>();
        if (newComps != null && newComps.Length != 0) components.AddRange(newComps);
        var children = gameObject.GetComponentsInChildren<Transform>();
        if (children != null && children.Length != 0)
        {
            foreach (var transform in children)
            {
                if (transform.gameObject == gameObject) continue;
                components.AddRange(transform.gameObject.GetAllNestedComponentsOfType<T>());
            }
        }

        return components;
    }
}