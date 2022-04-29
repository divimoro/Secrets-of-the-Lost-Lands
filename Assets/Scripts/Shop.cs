using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject[] weaponList;                       //list to all the 3D models of items
    public ShopData shopData;    
    
    private SaveSystem _saveSystem;
    private PlayerData _playerData;
    
    private int _currentWeaponId = 0;                       //index of current item showing in the shop 
    private int _selectedWeaponId;                          //actual selected item index
    public Text unlockBtnText, upgradeBtnText,equipBtnText;
    public Text unlockBtnTextShadow, upgradeBtnTextShadow,equipBtnTextShadow;
    public GameObject upgradeBtnCoin;
    public Text damageText,damageTextShadow,totalCoinsText;
    public Button unlockBtn, upgradeBtn, nextBtn, previousButton, equipBtn;   //ref to important Buttons
    private void OnEnable()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        _playerData = _saveSystem.LoadData();
        LoadShop();
    }

    public void Initialize()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        if (PlayerPrefs.GetInt("GameStartFirstTime") == 1) 
        {
            shopData = _saveSystem.LoadShopData();  
        }
        else                                               
        {
            _saveSystem.SaveShopData(shopData);                             
            PlayerPrefs.SetInt("GameStartFirstTime", 1);  
        }
    }

    private void LoadShop()
    {
        Initialize();
        
        _selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0);
        _currentWeaponId = _selectedWeaponId;

        totalCoinsText.text = "" + _playerData.Coins;
        SetWeaponInfo();

        equipBtn.onClick.AddListener(() => UnlockSelectButton());   
        unlockBtn.onClick.AddListener(() => UnlockSelectButton());      //add listner to button
        upgradeBtn.onClick.AddListener(() => UpgradeButton());          //add listner to button
        nextBtn.onClick.AddListener(() => NextButton());                //add listner to button
        previousButton.onClick.AddListener(() => PreviousButton());     //add listner to button

        previousButton.interactable = _currentWeaponId != 0;
       
        nextBtn.interactable = _currentWeaponId != shopData.shopItems.Length - 1;

        for (int i = 0; i < weaponList.Length; i++)
        {
            weaponList[i].SetActive(i == _currentWeaponId);
        }
       
                        
        UnlockButtonStatus();                                           
        UpgradeButtonStatus();
    }

    private void SaveShop()
    {
        _saveSystem.SaveShopData(shopData);     
        _saveSystem.SaveData(_playerData);
        
    }
  
    
    void SetWeaponInfo()
    {
        int currentLevel = shopData.shopItems[_currentWeaponId].unlockedLevel;
        damageText.text = "Damage: " + shopData.shopItems[_currentWeaponId].weaponLevelsData[currentLevel].damage;
        damageTextShadow.text = "Damage: " + shopData.shopItems[_currentWeaponId].weaponLevelsData[currentLevel].damage;
    }
    
    private void NextButton()
    {
        //check if currentIndex is less than the total shope items we have - 1
        if (_currentWeaponId < shopData.shopItems.Length - 1)
        {
            weaponList[_currentWeaponId].SetActive(false);                     //deactivate old model
            _currentWeaponId++;                                             //increase count by 1
            weaponList[_currentWeaponId].SetActive(true);                      //activate the new model
            SetWeaponInfo();                                               //set car information

            //check if current index is equal to total items - 1
            if (_currentWeaponId == shopData.shopItems.Length - 1)
            {
                nextBtn.interactable = false;                           //then set nextBtn interactable false
            }

            if (!previousButton.interactable)                           //if previousButton is not interactable
            {
                previousButton.interactable = true;                     //then set it interactable
            }

            UnlockButtonStatus();
            UpgradeButtonStatus();
        }
    }
    
    private void PreviousButton()
    {
        if (_currentWeaponId > 0)                           //we check is currentIndex i more than 0
        {
            weaponList[_currentWeaponId].SetActive(false);     //deactivate old model
            _currentWeaponId--;                             //reduce count by 1
            weaponList[_currentWeaponId].SetActive(true);      //activate the new model
            SetWeaponInfo();                               //set car information

            if (_currentWeaponId == 0)                      //if currentIndex is 0
            {
                previousButton.interactable = false;    //set previousButton interactable to false
            }

            if (!nextBtn.interactable)                  //if nextBtn interactable is false
            {
                nextBtn.interactable = true;            //set nextBtn interactable to true
            }
            UnlockButtonStatus();
            UpgradeButtonStatus();
        }
    }
    
    private void UnlockSelectButton()
    {
        equipBtn.interactable = true;
        bool yesSelected = false;   //local bool
        if (shopData.shopItems[_currentWeaponId].isUnlocked)    //if shop item at currentIndex is already unlocked
        {
            yesSelected = true;                             //set yesSelected to true
        }
        else if (!shopData.shopItems[_currentWeaponId].isUnlocked)  //if shop item at currentIndex is not unlocked
        {
            //check if we have enough coins to unlock it
            if (_playerData.Coins >= shopData.shopItems[_currentWeaponId].unlockCost)
            {
                //if yes then reduce the cost coins from our total coins
                _playerData.Coins -= shopData.shopItems[_currentWeaponId].unlockCost;
                totalCoinsText.text = "" + _playerData.Coins;          //set the coins text
                yesSelected = true;                             //set yesSelected to true
                shopData.shopItems[_currentWeaponId].isUnlocked = true; //mark the shop item unlocked
                UpgradeButtonStatus();
                SaveShop();
            }
        }

        if (yesSelected)
        {
            equipBtn.interactable = false;
            unlockBtn.gameObject.SetActive(false);
            equipBtn.gameObject.SetActive(true);
            equipBtnText.text = "Equiped";
            equipBtnTextShadow.text = "Equiped";
            _selectedWeaponId = _currentWeaponId;
            PlayerPrefs.SetInt("SelectedWeaponId", _selectedWeaponId);
           
        }
    }   
    
    private void UpgradeButton()//upgrade button is interactable only if we have any level left to upgrade
    {
        //get the next level index
        int nextLevelIndex = shopData.shopItems[_currentWeaponId].unlockedLevel + 1;
        //we check if we have enough coins
        if (shopData.shopItems[_currentWeaponId].unlockedLevel < shopData.shopItems[_currentWeaponId].weaponLevelsData.Length - 1 && _playerData.Coins >= shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex].unlockCost)
        {
            _playerData.Coins -= shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex].unlockCost;
            UpgradeButtonStatus();
            totalCoinsText.text = "" + _playerData.Coins;      
           
            shopData.shopItems[_currentWeaponId].unlockedLevel++;

            //we check if are not at max level
            if (shopData.shopItems[_currentWeaponId].unlockedLevel < shopData.shopItems[_currentWeaponId].weaponLevelsData.Length - 1)
            {
                upgradeBtnCoin.SetActive(true);
                upgradeBtnText.text = ""+shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex + 1].unlockCost;
                upgradeBtnTextShadow.text = ""+shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex + 1].unlockCost;
            }
            else    //we check if we are at max level
            {
                upgradeBtn.interactable = false;
                upgradeBtnText.text = "Max";
                upgradeBtnTextShadow.text = "Max";
                upgradeBtnCoin.SetActive(false);
            }

            SetWeaponInfo();
            SaveShop();
        }
        else
        {
            upgradeBtn.interactable = false;
        }
    }
    
    private void UnlockButtonStatus()
    {
        //if current item is unlocked
        if (shopData.shopItems[_currentWeaponId].isUnlocked)
        {
            //if selectedIndex is not equal to currentIndex set unlockBtn interactable false else make it true
            equipBtn.interactable = _selectedWeaponId != _currentWeaponId ? true : false;
            unlockBtn.gameObject.SetActive(false);
            equipBtn.gameObject.SetActive(true);
            //set the text
            equipBtnText.text = _selectedWeaponId == _currentWeaponId ? "Equiped" : "Equip";
            equipBtnTextShadow.text = _selectedWeaponId == _currentWeaponId ? "Equiped" : "Equip";
        }
        else if (!shopData.shopItems[_currentWeaponId].isUnlocked) //if current item is not unlocked
        {
            if(_playerData.Coins >= shopData.shopItems[_currentWeaponId].unlockCost)
            {
                unlockBtn.interactable = true;
            }
            else
            {
                unlockBtn.interactable = false;
            }
            unlockBtn.gameObject.SetActive(true);
            equipBtn.gameObject.SetActive(false);
            unlockBtnText.text = shopData.shopItems[_currentWeaponId].unlockCost + "";
            unlockBtnTextShadow.text = shopData.shopItems[_currentWeaponId].unlockCost + "";
        }
    }
    
    private void UpgradeButtonStatus()
    {
        //if current item is unlocked
        if (shopData.shopItems[_currentWeaponId].isUnlocked)
        {
            //if unlockLevel of current item is less than its max level
            if (shopData.shopItems[_currentWeaponId].unlockedLevel < shopData.shopItems[_currentWeaponId].weaponLevelsData.Length - 1)
            {
                int nextLevelIndex = shopData.shopItems[_currentWeaponId].unlockedLevel + 1;

                if (_playerData.Coins >= shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex].unlockCost)
                {
                    upgradeBtn.interactable = true;
                }
                else
                {
                    upgradeBtn.interactable = false;
                }
                upgradeBtnCoin.SetActive(true);
                upgradeBtnText.text = ""+shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex].unlockCost;
                upgradeBtnTextShadow.text = ""+shopData.shopItems[_currentWeaponId].weaponLevelsData[nextLevelIndex].unlockCost;
            }
            else   
            {
                upgradeBtn.interactable = false;                  
                upgradeBtnText.text = "Max";
                upgradeBtnTextShadow.text = "Max";
                upgradeBtnCoin.SetActive(false);
            }
        }
        else if (!shopData.shopItems[_currentWeaponId].isUnlocked)  
        {
            //upgradeBtnCoin.SetActive(false);
            upgradeBtn.interactable = false;                     
            upgradeBtnText.text = "Locked";
            upgradeBtnTextShadow.text = "Locked";
        }
    }
    
}
