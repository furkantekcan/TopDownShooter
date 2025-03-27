using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float damageRange;

    private float speed;
    private bool isDead;
    private float fullHealth;
    private bool isAttacking;
    private bool isMoving;
    private bool isTouching;

    private Rigidbody rb;
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyAnimation = GetComponent<EnemyAnimation>();    

        isDead = false;
        isAttacking = false;
        speed = moveSpeed;
        fullHealth = health;
    }

    private void Update()
    {
        if (!isDead)
        {
            Handlemovement();
            HandleAttack();
        }
    }

    public void RenderDamageEffect(Vector3 position,Vector3 normal)
    {
        var effect = Instantiate(damageEffect, position, Quaternion.identity);
        effect.transform.forward = normal;
        Destroy(effect,0.2f);
    }

    public void UpdateHealthBar()
    {
        healthImage.fillAmount = health / fullHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            enemyAnimation.HitAnim();
            health -= damage;
            UpdateHealthBar();

            if (health <= 0 && !isDead)
            {
                Dead();
            }

            speed = 0;

            Invoke("ResetSpeed", 0.1f);
        }
    }

    public void Dead()
    {
        Debug.Log("Enemy died!");
        moveSpeed = 0f;
        isAttacking = false;
        isDead = true;
        //GetComponent<EnemySpiderAnimation>().DeadAnim();
        GetComponent<CapsuleCollider>().enabled = false;
        rb.isKinematic = true;

        rb.AddForce(-transform.forward * 100, ForceMode.Impulse);
        GetComponent<RagdollPhysics>().RagdollOn();

        EnemyManager.Instance.CheckEnemyCount();

        Destroy(this.gameObject, 2f);
    }

    public void Handlemovement()
    {
        isAttacking = false;
        transform.LookAt(PlayerController.Instance.transform.position);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void HandleAttack()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= damageRange)
        {
            Stop();
            enemyAnimation.CloseAttackAnim();

            //Damage function is working with animation event.....
        }
        else if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= 5f && Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > damageRange)
        {
            enemyAnimation.RangeAttackAnim();
            ResetSpeed();
        }
        else
        {
            ResetSpeed();
        }
    }

    public void GiveDamage()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= damageRange) {
        PlayerController.Instance.TakeDamage(damage);
        }
    }

    private void ResetSpeed()
    {
        speed = moveSpeed;
    }

    public void Stop()
    {
        speed = 0f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
