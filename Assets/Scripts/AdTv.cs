using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdTv : MonoBehaviour
{
    [SerializeField] private GameObject messageUI;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject healthPotionPrefab;
    [SerializeField] private GameObject energyPotionPrefab;
   
    private bool _isActive = true;
    private int _countCoins;
   
    private const int _minCoins = 50;
    private const int _maxCoins = 100;
    private const int _countHealthPotions = 2;
    private const int _countEnergyPotions = 2;
    private RewardedAd _rewardedAd;
    public GameObject MessageUI => messageUI;
    

    public void ShowReward()
    {
        _rewardedAd = GetComponent<RewardedAd>();
        _rewardedAd.ShowAd();
    }
    public async UniTask DropItems()
    {
        
        await UniTask.Delay(300);
        _countCoins = Random.Range(_minCoins, _maxCoins);

        for (int i = 0; i < _countCoins; i++)
        {
            SpawnItem(coinPrefab);
        }
        for (int i = 0; i < _countHealthPotions; i++)
        {
            SpawnItem(healthPotionPrefab);
        }
        for (int i = 0; i < _countEnergyPotions; i++)
        {
            SpawnItem(energyPotionPrefab);
        }
        
        Destroy(gameObject);
    }

    private void SpawnItem(GameObject prefab)
    {
        var position = transform.position;
        var spawnItem = Instantiate(prefab, new Vector3(position.x,position.y+1f), Quaternion.identity,transform.parent);
        Rigidbody2D rb = spawnItem.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * Random.Range(2f,7f), ForceMode2D.Impulse);
        rb.AddForce(transform.right * Random.Range(-3f,3f), ForceMode2D.Impulse);
    }
}
