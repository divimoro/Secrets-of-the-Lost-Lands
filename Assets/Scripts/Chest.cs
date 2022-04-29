using Cysharp.Threading.Tasks;
using UnityEngine;



public class Chest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject messageUI;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject healthPotionPrefab;
    [SerializeField] private GameObject energyPotionPrefab;
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 3;
    
    private int _countCoins;
    private bool _isActive = true;

    public GameObject MessageUI => messageUI;

    public bool IsActive => _isActive;

    public async UniTask DropItems()
    {
        if(!IsActive)
            return;
        
        animator.SetTrigger("OpenChest");
        await UniTask.Delay(300);
        _countCoins = Random.Range(minCoins, maxCoins);

        for (int i = 0; i < _countCoins; i++)
        {
            SpawnItem(coinPrefab);
        }
        SpawnItem(healthPotionPrefab);
        SpawnItem(energyPotionPrefab);
        _isActive = false;
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
