using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityAnalytics unityAnalytics;
    [SerializeField] private Shop shop;
    [SerializeField] private AudioMixer audioMixer;
    private UIController uiController;
    private SaveSystem saveSystem;
    private PlayerData _playerData;
    private LevelData _levelData;
    private LevelManager levelManager;
    private LevelSelection _levelSelection;
    private CameraController cameraController;
    private MobileControls _mobileControls;
    private UILevelCompleteScore _uiLevelCompleteScore;

    public int Coins => _playerData.Coins;
    public int LastOpenedLevel => _playerData.LastOpenedLevel;
    public event Action<int> OnCoinsCountChanged = null;
    private bool _isCanResume;
    private float _startLevelTime;
    private float _endLevelTime;
    private float _levelLifeTime;
    private float _currentVolume;
    private void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        saveSystem = GetComponent<SaveSystem>();
        levelManager = GetComponent<LevelManager>();
        cameraController = FindObjectOfType<CameraController>();
        _playerData = saveSystem.LoadData();
        uiController.ShowMainMenuScreen();
        //InterstitialAd.S.LoadAd();
       // RewardedAd.S.LoadAd();
    }
    private void OnApplicationQuit()
    {
        /*if (_playerData != null)
        {
            saveSystem.SaveData(_playerData);
        }
        if (_levelData != null)
        {
            saveSystem.SaveLevelData(_levelData);
        }*/
    }

    private void Start()
    {
        _currentVolume = PlayerPrefs.GetFloat("AudioVolume", 1);
        var currentVolume = Mathf.Log10(_currentVolume) * 20;
        audioMixer.SetFloat("volume",currentVolume);
    }

    public void OpenMainMenu()
    {
        levelManager.ClearLevel();
        Time.timeScale = 1f;
        uiController.ShowMainMenuScreen();
    }
    public void StartGame(int level)
    {
        unityAnalytics.OnLevelStarted(level);
        _startLevelTime = Time.time;
        LoadLevel(level);
    }

    public void RestartGame()
    {
        if(!_isCanResume)
            return;
        
        _isCanResume = false;
        LoadLevel(LastOpenedLevel);
    }

    private void LoadLevel(int level)
    {
        uiController.ShowGameScreen();
        shop.Initialize();
        levelManager.InstantiateLevel(level);
        _levelData = saveSystem.LoadLevelData();
        _playerData = saveSystem.LoadData();
        OnGameStarted();
    }
    public void PauseGame()
    {
        uiController.ShowPauseScreen();
        StopGameTime();
    }
    public void ResumeGame()
    {
        if(!_isCanResume)
            return;

        _isCanResume = false;
        uiController.ShowGameScreen();
        Time.timeScale = 1f;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    public void NextLevel()
    {
        LevelSelection();
    }
    private async UniTask StopGameTime()
    {
        await UniTask.Delay(1000);
        Time.timeScale = 0f;
        _isCanResume = true;
    }
    public void GameOver()
    {
        unityAnalytics.OnPlayerDead(levelManager.CurrentLevelIndex);
        InterstitialAd.S.ShowAd();
        uiController.ShowGameOverScreen();
        StopGameTime();
        OnGameEnded();
    }


    private void LevelComplete()
    {
        
        _endLevelTime = Time.time;
        _levelLifeTime = _endLevelTime - _startLevelTime;
        unityAnalytics.OnLevelLifetime(levelManager.CurrentLevelIndex,_levelLifeTime.ToString());
        
        var currentLevel = _levelData.Levels.First(lvl => lvl.levelIndex == levelManager.CurrentLevelIndex);
        if (currentLevel.levelIndex != _levelData.Levels.Count)
        {
            var nextLevel = _levelData.Levels.First(lvl => lvl.levelIndex == levelManager.CurrentLevelIndex+1);
            nextLevel.isUnlocked = true;
        }

        uiController.ShowLevelCompleteScreen();
        _isCanResume = true;
        
        _uiLevelCompleteScore = uiController.GetComponentInChildren<UILevelCompleteScore>();
        var score = _uiLevelCompleteScore.GetLevelCoins(levelManager.KilledEnemyCount, levelManager.TotalEnemyCount);
        
        _playerData.Coins += score["coins"];

        if(currentLevel.stars < score["stars"])
            currentLevel.stars = score["stars"];
        
        if (currentLevel.levelIndex == _playerData.LastOpenedLevel)
        {
            _playerData.LastOpenedLevel++;
        }

        if (_playerData != null)
        {
            saveSystem.SaveData(_playerData);
        }
        if (_levelData != null)
        {
            saveSystem.SaveLevelData(_levelData);
        }
        
        OnGameEnded();
    }

    public void LevelSelection()
    {
        levelManager.ClearLevel();
        Time.timeScale = 1f;
        uiController.ShowLevelSelectionScreen();
    }

    public void OpenShop()
    {
        uiController.ShowShopScreen();
    }
    public void OpenSettings()
    {
        uiController.ShowSettingsScreen();
    }
    private void OnGameStarted()
    {
        Time.timeScale = 1f;
        cameraController.Initialize(levelManager.PlayerControl.transform);
        
        _mobileControls = FindObjectOfType<MobileControls>();
        _mobileControls.Initialize(levelManager.PlayerControl,levelManager.AttackControl);
        
        levelManager.PlayerControl.OnWin += LevelComplete;
        levelManager.PlayerControl.OnLost += GameOver;
        levelManager.PlayerControl.OnCoinCollected += OnCoinsCollected;
    }
    private void OnGameEnded()
    {
        levelManager.PlayerControl.OnWin -= LevelComplete;
        levelManager.PlayerControl.OnLost -= GameOver;
        levelManager.PlayerControl.OnCoinCollected -= OnCoinsCollected;
    }
    private void OnCoinsCollected()
    {
        _playerData.Coins++;
        OnCoinsCountChanged?.Invoke(Coins);
    }
}
