using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterEnergy : MonoBehaviour
{
    private Slider _slider;
    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }
    public void OnEnergyChange(float value)
    {
        _slider.value = value;
    }
}
