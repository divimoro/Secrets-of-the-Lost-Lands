using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
   

    [SerializeField] private float attackRate = 2f;
    
    private SaveSystem _saveSystem;
    private ShopData _shopData;
    
    private float _nextAttackTime = 0f;
    private bool _isAttack; 
    public bool IsAttack => _isAttack;
    
    private int _attackDamage;
    private int _selectedWeaponId;  
    private int _weaponLevelId;  
    private void Start()
    {
        _selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0);
        _saveSystem = FindObjectOfType<SaveSystem>();
        _shopData = _saveSystem.LoadShopData();
        _weaponLevelId = _shopData.shopItems[_selectedWeaponId].unlockedLevel;
        _attackDamage = _shopData.shopItems[_selectedWeaponId].weaponLevelsData[_weaponLevelId].damage;
    }

    public void FinishAttack()
    {
        _isAttack = false;
    }
  
    public void Attack()
    {
        if (Time.time >= _nextAttackTime)
        {
            _isAttack = true;
            animator.SetTrigger("attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                var enemyHealth = enemy.gameObject.GetComponentInParent<EnemyHealth>();
                enemyHealth.ReduceHealth(_attackDamage);
            }
            
            attackSound.Play();
            _nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
