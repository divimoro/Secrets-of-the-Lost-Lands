using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int totalEnergy = 100;
    
    private UICharacterEnergy _uiCharacterEnergy;

    private int _energy;
    private bool _isIncreaseEnergy;

    public int Energy => _energy;

    private void Awake()
    {
        _uiCharacterEnergy = FindObjectOfType<UICharacterEnergy>();
    }

    private void Start()
    {
        _energy = totalEnergy;
        SetEnergy();
    }
    
    public void ReduceEnergy(int value)
    {
        _energy -= value;
        SetEnergy();
    }
    public bool IncreaseEnergy(int value)
    {
        _isIncreaseEnergy = false;
        if (_energy != 100)
        {
            _energy += value;
            _isIncreaseEnergy = true;
            SetEnergy();
        }
        return _isIncreaseEnergy;
    }
   
    private void SetEnergy()
    {
        _uiCharacterEnergy.OnEnergyChange(_energy);
    }
    
   
}
