using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private const string closeAttack = "CloseAttack";
    private const string rangeAttack = "RangeAttack";
    private const string isDead = "IsDead";
    private const string speed = "Speed";
    private const string isHit = "Hit";
    private const string random = "Random";

    private Animator animator;
    private Enemy enemy; 

    private void Start()
    {
        animator = GetComponentInChildren<Animator>(); 
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        animator.SetFloat(speed, enemy.GetSpeed());
    }

    public void RangeAttackAnim()
    {
        animator.SetTrigger(rangeAttack);
    }

    public void CloseAttackAnim()
    {
        animator.SetTrigger(closeAttack);
    }

    public void DeadAnim()
    {
        //animator.SetBool(isDead, enemy.IsDead());
        //animator.SetBool(isDead, false);
        animator.SetTrigger(isDead);
        animator.SetBool("Death", enemy.IsDead());
    }

    public void HitAnim()
    {
        animator.SetTrigger(isHit);
    }
}
