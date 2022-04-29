using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private UIPlayerCoins _uiPlayerCoins;
    private GameManager _gameManager;
   
    private void Awake()
    {
        _uiPlayerCoins = FindObjectOfType<UIPlayerCoins>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        _uiPlayerCoins.OnCoinsChange(_gameManager.Coins);
        _gameManager.OnCoinsCountChanged += _uiPlayerCoins.OnCoinsChange;
    }
    
    private void OnDestroy()
    {
        _gameManager.OnCoinsCountChanged -= _uiPlayerCoins.OnCoinsChange;
    }
}
