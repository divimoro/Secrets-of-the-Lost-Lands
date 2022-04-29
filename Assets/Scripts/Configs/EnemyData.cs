using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "Configs/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Model Settings")]
    [SerializeField] private GameObject model;
   
    [Header("Chasing Settings")]
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float timeToWait = 5f;
    [SerializeField] private float timeToChase = 3f;
    [SerializeField] private float chasingSpeed = 3f;
    
    [Header("Vision Settings")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float circleRadius;
    [SerializeField] private float maxDistance;
    
    [Header("Attack Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float timeToDamage = 1f;
    
    [Header("Health Settings")]
    [SerializeField] private int totalHealth = 100;
    
    [Header("Effects Settings")]
    [SerializeField] private GameObject takeDamageFX;
    [SerializeField] private GameObject dieFX;
    [SerializeField] private AudioClip enemyHitSound;
    
    [Header("Drop Settings")]
    [SerializeField] private int minCoinsDrop;
    [SerializeField] private int maxCoinsDrop;
    
    public GameObject Model => model;
    public float WalkDistance => walkDistance;
    public float PatrolSpeed => patrolSpeed;
    public float TimeToWait => timeToWait;
    public float TimeToChase => timeToChase;
    public float ChasingSpeed => chasingSpeed;
    public float CircleRadius => circleRadius;
    public float MaxDistance => maxDistance;
    public LayerMask LayerMask => layerMask;
    public int Damage => damage;
    public float TimeToDamage => timeToDamage;
    public int TotalHealth => totalHealth;
    public GameObject TakeDamageFX => takeDamageFX;
    public GameObject DieFX => dieFX;
    public AudioClip EnemyHitSound => enemyHitSound;
    public int MinCoinsDrop => minCoinsDrop;
    public int MaxCoinsDrop => maxCoinsDrop;
}
