using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;
    
    private ShopData _shopData;
    private LevelData _levelData;
    private SaveSystem _saveSystem;
    private GameObject _currentLevel;
    private int _totalEnemyCount;
    private int _killedEnemyCount;
    private int _currentLevelIndex;
    private int _selectedWeaponId;
    private Transform _startPlayerPosition;
    private GameObject _playerModel;
    
    private PlayerController _playerControlController;
    private AttackController _attackControlController;
    private EnemyHealth[] _enemyHealth;
    
    
    public PlayerController PlayerControl => _playerControlController;
    public AttackController AttackControl => _attackControlController;
    public int TotalEnemyCount => _totalEnemyCount;
    public int KilledEnemyCount => _killedEnemyCount;

    public int CurrentLevelIndex => _currentLevelIndex;

    private void Start()
    {
        _saveSystem = GetComponent<SaveSystem>();
        _levelData = _saveSystem.LoadLevelData();
        LoadLevelsData();
    }

    private void OnDestroy()
    {
        if (_enemyHealth != null)
        {
            for (int i = 0; i < _enemyHealth.Length; i++)
            {
                _enemyHealth[i].OnDie -= OnEnemyDie;
            }
        }
    }

    private void OnEnemyDie()
    {
        _killedEnemyCount++;
    }

    public void InstantiateLevel(int index)
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        
        _shopData = _saveSystem.LoadShopData();
        _selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0);
        _playerModel = _shopData.shopItems[_selectedWeaponId].skinModel;
        
        _currentLevel = Instantiate(levels[index-1], transform);
        _startPlayerPosition = _currentLevel.transform.Find("PlayerStartPosition").transform;
        var startPosition = _startPlayerPosition.position;
        
        var player = Instantiate(_playerModel, new Vector3(startPosition.x,startPosition.y,startPosition.z),quaternion.identity,_startPlayerPosition.parent);
        
        _enemyHealth = _currentLevel.gameObject.GetComponentsInChildren<EnemyHealth>();
        if (_enemyHealth != null)
        {
            for (int i = 0; i < _enemyHealth.Length; i++)
            {
                _enemyHealth[i].OnDie += OnEnemyDie;
            }
            _totalEnemyCount = _enemyHealth.Length;
        }
      
        
        _killedEnemyCount = 0;
        
        _playerControlController = player.GetComponent<PlayerController>();
        _attackControlController = player.GetComponent<AttackController>();
        _currentLevelIndex = index;
    }

    public void ClearLevel()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
    }
    private void LoadLevelsData()
    {
        if (_levelData.Levels.Count == 0)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                var newLevel = new Level();
                newLevel.stars = 0;
                newLevel.levelIndex = i+1;
                newLevel.isUnlocked = false;
                if (i == 0) newLevel.isUnlocked = true;
                
                _levelData.Levels.Add(newLevel);
            }
            _saveSystem.SaveLevelData(_levelData);
        }
        
    }
   
}
