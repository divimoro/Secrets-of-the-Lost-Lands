using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
   
    
    private const string PLAYER_DATA_KEY = "GAME_DATA";
    private const string LEVEL_DATA_KEY = "LEVEL_DATA";
    private const string SHOP_DATA_KEY = "SHOP_DATA";

    public PlayerData LoadData()
    {
        if (PlayerPrefs.HasKey(PLAYER_DATA_KEY))
        {
            return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(PLAYER_DATA_KEY));
        }
        else
        {
            return new PlayerData();
        }
    }

    public void SaveData(PlayerData playerData)
    {
        PlayerPrefs.SetString(PLAYER_DATA_KEY,JsonUtility.ToJson(playerData));
    }
    
    
    
    public LevelData LoadLevelData()
    {
        if (PlayerPrefs.HasKey(LEVEL_DATA_KEY))
        {
            return JsonUtility.FromJson<LevelData>(PlayerPrefs.GetString(LEVEL_DATA_KEY));
        }
        else
        {
            return new LevelData();
        }
    }

    public void SaveLevelData(LevelData levelData)
    {
        PlayerPrefs.SetString(LEVEL_DATA_KEY,JsonUtility.ToJson(levelData));
    }
    
    
    public ShopData LoadShopData()
    {
        if (PlayerPrefs.HasKey(SHOP_DATA_KEY))
        {
            return JsonUtility.FromJson<ShopData>(PlayerPrefs.GetString(SHOP_DATA_KEY));
        }
        else
        {
            return new ShopData();
        }
    }

    public void SaveShopData(ShopData shopData)
    {
        PlayerPrefs.SetString(SHOP_DATA_KEY,JsonUtility.ToJson(shopData));
    }
}

[Serializable]
public class PlayerData
{
    public int Coins = 0;
    public int LastOpenedLevel = 1;
}
[Serializable]
public class Level
{
    public int levelIndex = 0;
    public int stars = 0;
    public bool isUnlocked;
}
[Serializable]
public class LevelData
{
    public List<Level> Levels = new List<Level>();
}


[Serializable]
public class ShopData
{
    public ShopItem[] shopItems;
}

[Serializable]
public class ShopItem
{
    public GameObject weaponModel;
    public GameObject skinModel;
    public string weaponName; 
    public bool isUnlocked;
    public int unlockCost;
    public int unlockedLevel = 1; 
    public WeaponLevelData[] weaponLevelsData;
}

[Serializable]
public class WeaponLevelData
{
    public int unlockCost;
    public int damage;
}
