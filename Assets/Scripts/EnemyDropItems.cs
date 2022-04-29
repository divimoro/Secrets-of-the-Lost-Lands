using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDropItems : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;

    private EnemyController _enemyController;
    private int _countCoins;
    private int _minCoins;
    private int _maxCoins;
   
    private void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }
   
    public void DropItems()
    {
        
        _minCoins = _enemyController.EnemyData.MinCoinsDrop;
        _maxCoins = _enemyController.EnemyData.MaxCoinsDrop;
        
        _countCoins = Random.Range(_minCoins, _maxCoins);

        for (int i = 0; i < _countCoins; i++)
        {
            SpawnItem(coinPrefab);
        }
    }
    private void SpawnItem(GameObject prefab)
    {
        var spawnItem = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
        Rigidbody2D rb = spawnItem.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * Random.Range(2f,7f), ForceMode2D.Impulse);
        rb.AddForce(transform.right * Random.Range(-3f,3f), ForceMode2D.Impulse);
    }

   
}
