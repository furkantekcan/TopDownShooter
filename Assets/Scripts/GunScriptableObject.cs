using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 1)]

public class GunScriptableObject : ScriptableObject
{
    public GunSystem.GunType gunType;

    public float rpm;
    public float spread;

    //Gun Stats
    public int damagePerBullet;
    public float range;
    public float reloadTime;

    public int magazineSize;
    public int bulletsPerShot;

    //References
    public Transform firePoint;
    public GameObject impactEffect;
    public AudioClip shootingAudio;
    public AudioClip reloadAudio;

    public bool IsAuto()
    {
        return gunType == GunSystem.GunType.Auto;
    }
}
