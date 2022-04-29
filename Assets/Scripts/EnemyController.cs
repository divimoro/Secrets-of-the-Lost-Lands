using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    
    private static readonly int EnemyIsRun = Animator.StringToHash("isRun");
    private static readonly int EnemyIsWalk = Animator.StringToHash("isWalk");
    
    private Animator _animator;
    private Transform _enemyModelTransform;
    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _rightBoundaryPosition;
    private Vector2 _nextPoint;

    private bool _isFacingRight = true;
    private bool _isWait;
    private bool _isChasingPlayer;
    private bool _collidedWithPlayer;

    private float _walkSpeed;
    private float _waitTime;
    private float _chaseTime;
   
    public bool IsFacingRight => _isFacingRight;
    public EnemyData EnemyData => enemyData;
    
    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
        _rb = GetComponent<Rigidbody2D>();
        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * enemyData.WalkDistance;
        _waitTime = enemyData.TimeToWait;
        _chaseTime = enemyData.TimeToChase;
        _walkSpeed = enemyData.PatrolSpeed;
        
        
        if (enemyData != null)
            LoadEnemyModel(enemyData);
        
    }

    private void LoadEnemyModel(EnemyData data)
    {
        GameObject enemyModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
      
        _enemyModelTransform = enemyModel.transform.GetChild(0);
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(_isChasingPlayer)
        {
            StartChasingTimer();
        }
        if (_isWait && !_isChasingPlayer)
        {
            StartWaitTimer();
        }

        if (ShouldWait())
        {
            _isWait = true;
        }
    }
    private void FixedUpdate()
    {
        _nextPoint = Vector2.right * _walkSpeed * Time.fixedDeltaTime;
       
        if (_isChasingPlayer && _collidedWithPlayer)
        {
            return;
        }
        
        if (_isChasingPlayer)
        {
            ChasePlayer();
        }
        if (!_isWait && !_isChasingPlayer)
        {
            Patrol();
        }
    }
    
    private void Patrol()
    {
        _animator.SetBool(EnemyIsWalk, true);
        _animator.SetBool(EnemyIsRun, false);
        if (!_isFacingRight)
        {
            _nextPoint.x *= -1;
        }
        _rb.MovePosition((Vector2)transform.position + _nextPoint);
    }
    
    public void StartChasingPlayer()
    {
        _isChasingPlayer = true;
        _chaseTime = enemyData.TimeToChase;
        _walkSpeed = enemyData.ChasingSpeed;
    }

    private void ChasePlayer()
    {
        _animator.SetBool(EnemyIsWalk, false);
        _animator.SetBool(EnemyIsRun, true);
        
        float distance = DistanceToPlayer();
        if (distance < 0)
        {
            _nextPoint.x *= -1;
        }
        if (distance > 0.2f && !_isFacingRight)
        {
            Flip();
        }
        else if (distance < 0.2f && _isFacingRight)
        {
            Flip();
        }
        _rb.MovePosition((Vector2)transform.position + _nextPoint);
    }
    private float DistanceToPlayer()
    {
        return _playerTransform.position.x - transform.position.x;
    }
    private void StartWaitTimer()
    {
        _animator.SetBool(EnemyIsWalk, false);
        _animator.SetBool(EnemyIsRun, false);
        _waitTime -= Time.deltaTime;
        if (_waitTime < 0f)
        {
            _waitTime = enemyData.TimeToWait;
            _isWait = false;
            Flip();
        }
    }
    private void StartChasingTimer()
    {
        _chaseTime -= Time.deltaTime;
        if (_chaseTime < 0f)
        {
            _isChasingPlayer = false;
            _chaseTime = enemyData.TimeToChase;
            _walkSpeed = enemyData.PatrolSpeed;
           
        }
    }
    private bool ShouldWait()
    {
        bool isOutOfRightBoundary = _isFacingRight && transform.position.x >= _rightBoundaryPosition.x;
        bool isOutOfLeftBoundary = !_isFacingRight && transform.position.x <= _leftBoundaryPosition.x;
        return isOutOfLeftBoundary || isOutOfRightBoundary;
    }
   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition, _rightBoundaryPosition);
    }*/

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = _enemyModelTransform.localScale;
        playerScale.x *= -1;
        _enemyModelTransform.localScale = playerScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            _collidedWithPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            _collidedWithPlayer = false;
        }
    }
}
