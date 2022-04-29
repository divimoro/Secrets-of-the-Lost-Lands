using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterHealth : MonoBehaviour
{
    private Slider _slider;
    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }
    public void OnHealthChange(int value)
    {
        _slider.value = value;
    }
}
