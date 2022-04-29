using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
   [SerializeField] private GameObject gameOverCanvas;
   [SerializeField] private GameObject gameCanvas;
   [SerializeField] private GameObject pauseCanvas;
   [SerializeField] private GameObject levelCompleteCanvas;
   [SerializeField] private GameObject levelSelectionCanvas;
   [SerializeField] private GameObject shopCanvas;
   [SerializeField] private GameObject settingsCanvas;
   [SerializeField] private GameObject mainMenuCanvas;
   [SerializeField] private GameObject backgroundMenu;
   
   
   private GameObject _currenPanel;

   private void DisableCurrentPanel()
   {
      _currenPanel?.SetActive(false);
   }
   
   public void ShowMainMenuScreen()
   {
      backgroundMenu.SetActive(true);
      DisableCurrentPanel();
      _currenPanel = mainMenuCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowGameScreen()
   {
      backgroundMenu.SetActive(false);
      DisableCurrentPanel();
      _currenPanel = gameCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowGameOverScreen()
   {
      backgroundMenu.SetActive(false);
      DisableCurrentPanel();
      _currenPanel = gameOverCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowLevelCompleteScreen()
   {
      backgroundMenu.SetActive(false);
      DisableCurrentPanel();
      _currenPanel = levelCompleteCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowLevelSelectionScreen()
   {
      backgroundMenu.SetActive(true);
      DisableCurrentPanel();
      _currenPanel = levelSelectionCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowShopScreen()
   {
      backgroundMenu.SetActive(true);
      DisableCurrentPanel();
      _currenPanel = shopCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowSettingsScreen()
   {
      backgroundMenu.SetActive(true);
      DisableCurrentPanel();
      _currenPanel = settingsCanvas;
      _currenPanel.SetActive(true);
   }
   public void ShowPauseScreen()
   {
      backgroundMenu.SetActive(false);
      DisableCurrentPanel();
      _currenPanel = pauseCanvas;
      _currenPanel.SetActive(true);
   }
}
