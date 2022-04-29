using UnityEngine;

public class SpikesAttack : MonoBehaviour
{ 
    [SerializeField] private int damage;
    [SerializeField] private float timeToDamage;
    
    private bool _isDamage = true;
    private float _damageTime;
    private void Start()
    {
        _damageTime = timeToDamage;
    }
    private void Update()
    {
        if(!_isDamage)
        {
            _damageTime -= Time.deltaTime;
            if(_damageTime <= 0f)
            {
                _isDamage = true;
                _damageTime = timeToDamage;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null && _isDamage)
        {
            playerHealth.ReduceHealth(damage);
            _isDamage = false;
        }
    }
}
