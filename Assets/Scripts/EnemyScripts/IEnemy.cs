using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Handlemovement();
    public void TakeDamage(float damage);
    public void UpdateHealthBar();
    public void RenderDamageEffect(Vector3 position, Vector3 normal);
    public void Dead();
    public void Stop();
    public float GetSpeed();
    public bool IsAttacking();
}
