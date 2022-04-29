using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCoins : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    public void OnCoinsChange(int value)
    {
        text.text = value.ToString();
    }
    
}
