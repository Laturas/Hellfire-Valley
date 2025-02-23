using System;
using UnityEngine;

public class LeanTweenCanceler : MonoBehaviour
{
    private void Awake()
    {
        LeanTween.cancelAll();
        LeanTween.reset();
    }
}