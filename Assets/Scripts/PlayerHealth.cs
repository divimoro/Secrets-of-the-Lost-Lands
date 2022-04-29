using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject takeDamageFX;
    [SerializeField] private GameObject dieFX;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private int totalHealth = 100;
    
    private UICharacterHealth _uiCharacterHealth;
    
    public event Action OnDie;
    
    private int _health;
    private bool _isIncreaseHealth;
    private void Awake()
    {
        _uiCharacterHealth = FindObjectOfType<UICharacterHealth>();
    }

    private void Start()
    {
        _health = totalHealth;
        SetHealth();
    }
    
    public void ReduceHealth(int damage)
    {
        _health -= damage;
        hitSound.Play();
        TakeDamage();
        SetHealth();
        
        if (_health <= 0f)
        {
            OnDie?.Invoke();
            Die();
        }
    }
    public bool IncreaseHealth(int value)
    {
        _isIncreaseHealth = false;
        if (_health != 100)
        {
            _health += value;
            _isIncreaseHealth = true;
            SetHealth();
        }
        return _isIncreaseHealth;
    }
    private void SetHealth()
    {
        _uiCharacterHealth.OnHealthChange(_health);
    }

    private void Die()
    {
        if (dieFX)
             Instantiate(dieFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private async UniTask TakeDamage()
    {
        if (takeDamageFX)
            Instantiate(takeDamageFX, transform.position, Quaternion.identity);
        
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f,1f);
        await UniTask.Delay(500);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
