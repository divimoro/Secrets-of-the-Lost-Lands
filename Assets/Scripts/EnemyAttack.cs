using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
  
    private Animator _animator;
    
    private EnemyController _enemyController;

    private int _damage;
    private float _damageTime;
    private bool _isDamage = true;

    
    private void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        _animator = GetComponentInChildren<Animator>();
        
        _damage = _enemyController.EnemyData.Damage;
        _damageTime = _enemyController.EnemyData.TimeToDamage;
        
    }
    private void Update()
    {
        if(!_isDamage)
        {
            _damageTime -= Time.deltaTime;
            if(_damageTime <= 0f)
            {
                _isDamage = true;
                _damageTime = _enemyController.EnemyData.TimeToDamage;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null && _isDamage)
        {
            _animator.Play("Attack");
            playerHealth.ReduceHealth(_damage);
            _isDamage = false;
        }
    }
}
