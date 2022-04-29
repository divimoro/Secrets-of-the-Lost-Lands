using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UILevelCompleteScore : MonoBehaviour
{
   [SerializeField] private Text text;
   [SerializeField] private GameObject star1Full;
   [SerializeField] private GameObject star2Full;
   [SerializeField] private GameObject star3Full;
   [SerializeField] private GameObject star1Empty;
   [SerializeField] private GameObject star2Empty;
   [SerializeField] private GameObject star3Empty;
   
   private AudioSource _audioSource;
   private const int CoinsBonusPerStar = 25;
   private int _totalCoins;
   private int _totalCoinsBonus;
   private int _stars;
   private IDictionary<string,int> _levelScore;

   public IDictionary<string,int> GetLevelCoins(int killedCountEnemies, int totalCountEnemies)
   {
      _levelScore = new Dictionary<string, int>();
      _totalCoinsBonus = 0;
      _stars = 0;
      _audioSource = GetComponent<AudioSource>();
      
      star1Empty.SetActive(true);
      star2Empty.SetActive(true);
      star3Empty.SetActive(true);
      
      float percentageScore = (float) killedCountEnemies / totalCountEnemies;
      
      if (percentageScore <= 0.33f)
      {
         _totalCoins = 25;
         _stars = 1;
        
         star1Empty.SetActive(false);

         var s = DOTween.Sequence();
         s.Append(star1Full.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(PlayStarSound);
         s.AppendCallback(AddCoins);
         s.Append(text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
      }
      else if (percentageScore > 0.33f && percentageScore < 0.66f)
      {
         _totalCoins = 50;
         _stars = 2;
         
         star1Empty.SetActive(false);
         
         var s = DOTween.Sequence();
         s.Append(star1Full.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(PlayStarSound);
         s.AppendCallback(AddCoins);
         s.Append(text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(()=> star2Empty.SetActive(false));
         
         s.Append(star2Full.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(PlayStarSound);
         s.AppendCallback(AddCoins);
         s.Append(text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
      }
      else if (percentageScore >= 0.66f)
      {
         _totalCoins = 75;
         _stars = 3;
         
         star1Empty.SetActive(false);
  
         var s = DOTween.Sequence();
         s.Append(star1Full.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(PlayStarSound);
         s.AppendCallback(AddCoins);
         s.Append(text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(()=> star2Empty.SetActive(false));
         
         s.Append(star2Full.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(PlayStarSound);
         s.AppendCallback(AddCoins);
         s.Append(text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(()=> star3Empty.SetActive(false));
         
         s.Append(star3Full.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
         s.AppendCallback(PlayStarSound);
         s.AppendCallback(AddCoins);
         s.Append(text.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f),0.5f));
      }

      _levelScore["coins"] = _totalCoins;
      _levelScore["stars"] = _stars;
      
      return _levelScore;
   }
   
   private void PlayStarSound()
   {
      _audioSource.Play();
   }
   
   private void AddCoins()
   {
      _totalCoinsBonus += CoinsBonusPerStar;
      text.text = "+"+_totalCoinsBonus;
      
   }

}
