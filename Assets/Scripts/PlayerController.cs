using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerModelTransform;
    [SerializeField] private float speedX = 1f;
    [SerializeField] private float jumpImpulse = 10f;
    [SerializeField] private float rollDistance = 10f;
    [SerializeField] private float startRollTime;
   
    private static readonly int CharacterJump = Animator.StringToHash("jump");
    private static readonly int CharacterRoll = Animator.StringToHash("roll");
    private static readonly int SpeedX = Animator.StringToHash("speedX");
    
    private const float SpeedMultiplier = 100f;
    private const float RaycastCircleRadius = 0.5f;
    private const float RaycastCircleDistance = 0.5f;
    private const float StepAfterRoll = 2f;
    
    private Vector2 _origin;
    private Vector2 _direction;
    private LayerMask _playerLayer;
    private LayerMask _enemyLayer;
    
    private float _currentHitDistance;
    private float _rollTime;
    private float _horizontal;
    private bool _isGround;
    private bool _isJump;
    private bool _isRoll;
    private bool _isFacingRight = true;
    private bool _isActive = true;
    private bool _isLeverArm;
    private bool _isChest;
    private bool _isAdTv;
    private int _healthPerPotion = 20;
    private int _energyPerPotion = 20;
    private int _rollEnergyCost = 20;
    
    private Rigidbody2D _rb;
    private Finish _finish;
    private LeverArm _leverArm;
    private PlayerHealth _playerHealth;
    private PlayerEnergy _playerEnergy;
    private Chest _chest;
    private AdTv _adTv;
    public event Action OnWin, OnLost, OnCoinCollected;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerEnergy = GetComponent<PlayerEnergy>();
        _playerHealth.OnDie += OnDie;
        _playerLayer = LayerMask.NameToLayer("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        
    }

    private void Start()
    {
        _rollTime = startRollTime;
    }

    private void Update()
    {
        if(!_isActive)
            return;

        _horizontal = SimpleInput.GetAxis("Horizontal");
        animator.SetFloat(SpeedX, Mathf.Abs(_horizontal));
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }
    private void FixedUpdate()
    {
        if(!_isActive)
            return;
        
        _direction = _isFacingRight ? Vector2.right : Vector2.left;
        _origin = transform.position;
        
        _rb.velocity = new Vector2(_horizontal * speedX * SpeedMultiplier * Time.fixedDeltaTime, _rb.velocity.y);
        if (_isJump)
        {
            _rb.AddForce(Vector2.up * jumpImpulse,ForceMode2D.Impulse);
            _isGround = false;
            _isJump = false;
        }

        if (_isRoll)
        {
            if (_rollTime <= 0)
            {
                RaycastHit2D hit = Physics2D.CircleCast(_origin, RaycastCircleRadius, _direction, RaycastCircleDistance, _enemyLayer);
                _currentHitDistance = hit.distance;
                if (hit)
                {
                    _rb.velocity = _direction * rollDistance * StepAfterRoll * SpeedMultiplier * Time.fixedDeltaTime;
                }
                else
                {
                    _rollTime = startRollTime;
                    _rb.velocity = Vector2.zero;
                
                    _isRoll = false;
                    Physics2D.IgnoreLayerCollision(_playerLayer,_enemyLayer,false);

                }
            }
            else
            {
                _rollTime -= Time.fixedDeltaTime;
                _rb.velocity = _direction * rollDistance * SpeedMultiplier * Time.fixedDeltaTime;
                
            }
        }
        if (_horizontal > 0f && !_isFacingRight)
        {
            Flip();
        }
        else if (_horizontal < 0f && _isFacingRight)
        {
            Flip();
        }
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_origin, _origin + _direction * _currentHitDistance);
        Gizmos.DrawWireSphere(_origin + _direction  * _currentHitDistance, RaycastCircleRadius);
    }*/
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = playerModelTransform.localScale;
        playerScale.x *= -1;
        playerModelTransform.localScale = playerScale;
    }
    public void Jump()
    {
        if (!_isGround)
            return;
        
        if (Mathf.Abs(_rb.velocity.y) <= 2)
        {
            _isJump = true;
            animator.SetTrigger(CharacterJump);
            jumpSound.Play();
        }
    }
    public void Interact()
    {
        if (_isLeverArm)
        {
            _leverArm.ActivateLevelArm();
        }

        if (_isChest && _chest != null)
        {
            if (_chest.IsActive)
            {
                _chest.DropItems();
            }
        }
        
        if (_isAdTv && _adTv != null)
        {
            _adTv.ShowReward();
        }
    }
    
    public void Roll()
    {
        if(_isRoll || _playerEnergy.Energy < _rollEnergyCost)
           return;
      
        _isRoll = true;
        _playerEnergy.ReduceEnergy(_rollEnergyCost);
        Physics2D.IgnoreLayerCollision(_playerLayer,_enemyLayer);
        animator.SetTrigger(CharacterRoll);
        jumpSound.Play();
      
        
      
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            _isGround = true;
            Debug.Log("ground");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!_isActive)
            return;
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_isActive)
            return;
        
        
        var item = collision.gameObject.GetComponent<Item>();
        
        if (item != null)
        {
            switch (item.CollectibleType)
            {
                case Item.ItemTypes.Coin:
                    OnCoinCollected?.Invoke();
                    item.CollectEffect();
                    break;
                case Item.ItemTypes.Health:
                    if(_playerHealth.IncreaseHealth(_healthPerPotion))
                        item.CollectEffect();
                    break;
                case Item.ItemTypes.Energy:
                    if(_playerEnergy.IncreaseEnergy(_energyPerPotion))
                        item.CollectEffect();
                    break;
            }
            
        }
        
        LeverArm leverArmTemp = collision.GetComponent<LeverArm>();
        _chest = collision.GetComponent<Chest>();
        _adTv = collision.GetComponent<AdTv>();
        if (collision.CompareTag("Finish"))
        {
            Deactivate();
            OnWin?.Invoke();
        }
        if (leverArmTemp != null)
        {
            _isLeverArm = true;
        }
        if (_chest != null)
        {
            _isChest = true;
            if (_chest.IsActive)
            {
                _chest.MessageUI.SetActive(true);
                _chest.MessageUI.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
            }
        }
        if (_adTv != null)
        {
            _isAdTv = true;
            _adTv.MessageUI.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!_isActive)
            return;
        
        LeverArm leverArmTemp = collision.GetComponent<LeverArm>();
        _chest = collision.GetComponent<Chest>();
        _adTv = collision.GetComponent<AdTv>();
        
        if (leverArmTemp != null)
        {
            _isLeverArm = false;
        }
        if (_chest != null)
        {
            _isChest = false;
            _chest.MessageUI.SetActive(false);
        }
        if (_adTv != null)
        {
            _isAdTv = false;
            _adTv.MessageUI.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        }
    }
    private void Deactivate()
    {
        _isActive = false;
    }
    private void OnDestroy()
    {
        _playerHealth.OnDie -= OnDie;
    }
    private void OnDie()
    {
        Deactivate();
        OnLost?.Invoke();
    }
    
   
}
