using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public static GunSystem Instance;
    public enum GunType
    {
        Semi,
        Burst,
        Auto
    }

    public event EventHandler<OnBulletsChangedArgs> OnBulletsChangedAction;
    public class OnBulletsChangedArgs : EventArgs
    {
        public int bulletsInMagazine;
    }

    public event EventHandler OnReloadAction;

    private float timeBetweenShots;
    private float nextPossibleShootTime;
    private int bulletsLeft;

    //bools
    private bool shooting;
    private bool readyToShoot;
    private bool reloading;

    //References
    public Transform firePoint;
    public LayerMask enemyLayer;

    private LineRenderer lineRenderer;
    private RaycastHit rayHit;
    private AudioSource audioSource;

    public GunScriptableObject selectedGun;
        
    private void Awake()
    {
        Instance = this;

        GameInput.Instance.OnReloadAction += Instance_OnReloadAction;
        GameInput.Instance.OnShootingStartedAction += Instance_OnShootingStartedAction;
        GameInput.Instance.OnShootingCanceledAction += Instance_OnShootingCanceledAction;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnReloadAction -= Instance_OnReloadAction;
        GameInput.Instance.OnShootingStartedAction -= Instance_OnShootingStartedAction;
        GameInput.Instance.OnShootingCanceledAction -= Instance_OnShootingCanceledAction;
    }

    private void Start()
    {
        shooting = false;
        timeBetweenShots = 60 / selectedGun.rpm;

        bulletsLeft = selectedGun.magazineSize;
        readyToShoot = true;

        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = selectedGun.shootingAudio;

        OnBulletsChangedAction?.Invoke(this, new OnBulletsChangedArgs
        {
            bulletsInMagazine = bulletsLeft
        });
    }

    private void Instance_OnShootingCanceledAction(object sender, System.EventArgs e)
    {
        shooting = false;
    }

    private void Instance_OnShootingStartedAction(object sender, System.EventArgs e)
    {
        shooting = true;
    }

    private void Instance_OnReloadAction(object sender, System.EventArgs e)
    {
        Reload();
    }
    private void Update()
    {
        if (shooting)
        {
            if (selectedGun.IsAuto())
            {
                Shoot();
            }

            else
            {
                Shoot();
                shooting = false;
            }
        }
    }

    private void Shoot()
    {
        //spread
        float spread = selectedGun.spread;
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        Vector3 requiredPos = PlayerController.Instance.GetRequiredPosition();
        Vector3 direction = (requiredPos - firePoint.position) + new Vector3(x,y,0);

        if (CanShoot() && readyToShoot && bulletsLeft > 0)
        {
            if (Physics.Raycast(firePoint.position, direction, out rayHit, float.MaxValue))
            {
                if (rayHit.transform != null)
                {
                    StartCoroutine("RenderLine", rayHit.point - firePoint.position);

                    Debug.DrawRay(firePoint.position, rayHit.point - firePoint.position, Color.red, 1);

                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        rayHit.transform.GetComponent<Collider>().GetComponent<Enemy>().TakeDamage(selectedGun.damagePerBullet);
                        rayHit.transform.GetComponent<Collider>().GetComponent<Enemy>().RenderDamageEffect(rayHit.point, rayHit.normal);
                    }
                    else
                    {
                        //play impact effect
                        GameObject effect = Instantiate(selectedGun.impactEffect, rayHit.point, Quaternion.Euler(rayHit.normal));
                        Destroy(effect, 0.2f);
                    }
                }
                

            }

            else
            {
                StartCoroutine("RenderLine", new Ray(firePoint.position, direction).direction * 100);

                Debug.DrawRay(firePoint.position, (direction) * 100, Color.red, 1);
            }

            StartCoroutine("RenderMuzzleFlash");
            audioSource.Play();

            nextPossibleShootTime = Time.time + timeBetweenShots;

            bulletsLeft -= selectedGun.bulletsPerShot;
        }

        OnBulletsChangedAction?.Invoke(this, new OnBulletsChangedArgs
        {
            bulletsInMagazine = bulletsLeft
        });
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        return canShoot;
    }

    private void Reload()
    {
        if (bulletsLeft < selectedGun.magazineSize && !reloading)
        {
            //Reload
            Debug.Log("Reload");
            reloading = true;
            readyToShoot = false;
            StartCoroutine("PlayReloadAudio");
            OnReloadAction?.Invoke(this, EventArgs.Empty);  
            Invoke("ReloadFinished", selectedGun.reloadTime);
            
        }

        else
        {
            readyToShoot = true;
        }
    }

    private void ReloadFinished()
    {
        bulletsLeft = selectedGun.magazineSize;
        reloading = false;
        readyToShoot = true;
        OnBulletsChangedAction?.Invoke(this, new OnBulletsChangedArgs
        {
            bulletsInMagazine = bulletsLeft
        });
    }

    private IEnumerator PlayReloadAudio()
    {
        audioSource.clip = selectedGun.reloadAudio;
        audioSource.Play();
        yield return new WaitForSecondsRealtime(selectedGun.reloadTime);

        audioSource.clip = selectedGun.shootingAudio;
    }

    private IEnumerator RenderLine(Vector3 hitpoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position + new Vector3(hitpoint.x, 0, hitpoint.z));

        yield return null;

        lineRenderer.enabled = false;
    }

    private IEnumerator RenderMuzzleFlash()
    {
        firePoint.GetChild(0).gameObject.SetActive(true);

        yield return null;

        firePoint.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
    }

    public bool IsShooting()
    {
        return shooting;
    }

    public bool IsReloading()
    {
        return reloading;
    }
}
