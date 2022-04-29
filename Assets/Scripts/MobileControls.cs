using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    private AttackController _attackController;
    private PlayerController _playerController;
  
    public void Initialize(PlayerController playerController, AttackController attackController)
    {
        _playerController = playerController;
       _attackController = attackController;
    }
    public void Attack()
    {
        _attackController.Attack();
    }
    public void Jump()
    {
        _playerController.Jump();
    }
    public void Interact()
    {
        _playerController.Interact();
    }
    public void Roll()
    {
        _playerController.Roll();
    }
   
}
