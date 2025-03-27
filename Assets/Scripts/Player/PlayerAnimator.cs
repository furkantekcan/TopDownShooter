using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;

    private const string verticalSpeed = "Vertical";
    private const string horizontalSpeed = "Horizontal";
    private const string isReloading = "IsReloading";
    private const string isShooting = "IsShooting";
    private const string isDead = "IsDead";

    private void Awake()
    {
        GameInput.Instance.OnShootingStartedAction += Instance_OnShootingStartedAction;
        GameInput.Instance.OnShootingCanceledAction += Instance_OnShootingCanceledAction;
        
        GunSystem.Instance.OnReloadAction += Instance_OnReloadAction;

        PlayerController.Instance.OnDeathAction += DeathAnim;
    }

    private void Instance_OnReloadAction(object sender, System.EventArgs e)
    {
        ReloadAnimation();
    }

    private void Instance_OnShootingCanceledAction(object sender, System.EventArgs e)
    {
        SetShootAnimation();
    }

    private void Instance_OnShootingStartedAction(object sender, System.EventArgs e)
    {
        SetShootAnimation();
    }

    private void Start()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat(verticalSpeed, player.vertical);
        animator.SetFloat(horizontalSpeed, player.horizontal);
    }

    public void ReloadAnimation()
    {
        animator.SetTrigger(isReloading);
    }

    public void SetShootAnimation()
    {
        animator.SetBool(isShooting, GunSystem.Instance.IsShooting());
    }

    private void DeathAnim(object sender, System.EventArgs e)
    {
        animator.SetBool(isDead, true);
    }
}
