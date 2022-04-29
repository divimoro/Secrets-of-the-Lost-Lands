using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private EnemyController _enemyController;
    private Slider _healthSlider;
    private AudioSource _audioSource;
    private EnemyDropItems _enemyDropItems;
   // private LevelManager _levelManager;
    public event Action OnDie;
    private int _health;

    private void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        _audioSource = GetComponent<AudioSource>();
        _healthSlider = GetComponentInChildren<Slider>();
        _enemyDropItems = GetComponent<EnemyDropItems>();
       // _levelManager = FindObjectOfType<LevelManager>();
        
        _audioSource.clip = _enemyController.EnemyData.EnemyHitSound;
        _health = _enemyController.EnemyData.TotalHealth;
        
        SetHealth();
    }

    public void ReduceHealth(int damage)
    {
        _health -= damage;
        SetHealth();
        
        _audioSource.Play();
        if (_enemyController.EnemyData.TakeDamageFX)
            Instantiate(_enemyController.EnemyData.TakeDamageFX, transform.position, Quaternion.identity);
        
        if(_health <= 0f)
        {
            OnDie?.Invoke();
            Die();
        }
    }
    private void SetHealth()
    {
        _healthSlider.value = _health;
    }
    private void Die()
    { 
        if (_enemyController.EnemyData.DieFX)
            Instantiate(_enemyController.EnemyData.DieFX, transform.position, Quaternion.identity);
        
        gameObject.SetActive(false);
        _enemyDropItems.DropItems();
    }
}
