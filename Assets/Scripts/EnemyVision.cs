using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private EnemyController _enemyController;
    private GameObject _currentHitObject;
    
    private Vector2 _origin;
    private Vector2 _direction;
    private float _currentHitDistance;

    private void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }
    private void Update()
    {
        _origin = transform.position;

        if (_enemyController.IsFacingRight)
        {
            _direction = Vector2.right;
        }
        else
        {
            _direction = Vector2.left;
        }


        RaycastHit2D hit = Physics2D.CircleCast(_origin, _enemyController.EnemyData.CircleRadius, _direction, _enemyController.EnemyData.MaxDistance, _enemyController.EnemyData.LayerMask);

        if (hit)
        {
            _currentHitObject = hit.transform.gameObject;
            _currentHitDistance = hit.distance;
            if (_currentHitObject.CompareTag("Player"))
            {
                _enemyController.StartChasingPlayer();
            }
        }
        else
        {
            _currentHitObject = null;
            _currentHitDistance = _enemyController.EnemyData.MaxDistance;
        }


    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_origin, _origin + _direction * _currentHitDistance);
        Gizmos.DrawWireSphere(_origin + _direction * _currentHitDistance, _enemyController.EnemyData.CircleRadius);
    }*/


}
