using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
   [SerializeField] private GameManager gameManager;
   [SerializeField] private GameObject levelUnlocked;
   [SerializeField] private GameObject levelLocked;
   [SerializeField] private GameObject levelsGrid;
   
   private PlayerData _playerData;
   private LevelData _levelData;
   private SaveSystem _saveSystem;
   private List<Level> _levels;
 

   private void OnEnable()
   {
      foreach (Transform child in levelsGrid.transform) {
         Destroy(child.gameObject);
      }
      
      _saveSystem = FindObjectOfType<SaveSystem>();
      _playerData = _saveSystem.LoadData();
      _levelData = _saveSystem.LoadLevelData();
      _levels = _levelData.Levels;
      FillLevelList();
   }

   private void FillLevelList()
   {
      foreach (var level in _levels)
      {
         if (level.isUnlocked)
         {
            var levelButton = Instantiate(levelUnlocked,levelsGrid.transform);
            var button = levelButton.GetComponent<LevelSelectionButton>();
            button.levelText.text = level.levelIndex.ToString();
            button.GetComponent<Button>().onClick.AddListener(()=>gameManager.StartGame(level.levelIndex));
            if (level.stars == 0)
            {
               button.star1Empty.SetActive(false);
               button.star2Empty.SetActive(false);
               button.star3Empty.SetActive(false);
               button.star1Fill.SetActive(false);
               button.star2Fill.SetActive(false);
               button.star3Fill.SetActive(false);
            }
            else if (level.stars == 1)
            {
               button.star1Empty.SetActive(false);
               button.star1Fill.SetActive(true);
            }
            else if(level.stars == 2)
            {
               button.star1Empty.SetActive(false);
               button.star2Empty.SetActive(false);
               
               button.star1Fill.SetActive(true);
               button.star2Fill.SetActive(true);
            }
            else if(level.stars == 3)
            {
               button.star1Empty.SetActive(false);
               button.star2Empty.SetActive(false);
               button.star3Empty.SetActive(false);
               
               button.star1Fill.SetActive(true);
               button.star2Fill.SetActive(true);
               button.star3Fill.SetActive(true);
            }
         }
         else
         {
            var levelButton = Instantiate(levelLocked,levelsGrid.transform);
         }
        
      }
   }
   
}
