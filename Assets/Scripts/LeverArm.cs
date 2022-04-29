using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverArm : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Finish _finish;
    
    private void Start()
    {
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
    }
    public void ActivateLevelArm()
    {
        animator.SetTrigger("activate");
        //_finish.Activate();
    }
}
