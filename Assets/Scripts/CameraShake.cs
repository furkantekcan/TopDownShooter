using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private float shakeIntensity = 1.0f;
    private float shakeTime = 0.2f;

    private float timer;
    private bool shooting;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnDisable()
    {
        GameInput.Instance.OnShootingStartedAction -= Instance_OnShootingStartedAction;
        GameInput.Instance.OnShootingCanceledAction -= Instance_OnShootingCanceledAction;
    }

    private void Start()
    {
        GameInput.Instance.OnShootingStartedAction += Instance_OnShootingStartedAction;
        GameInput.Instance.OnShootingCanceledAction += Instance_OnShootingCanceledAction;
        
        StopShaking();
    }

    private void Instance_OnShootingCanceledAction(object sender, System.EventArgs e)
    {
        shooting = false;
    }

    private void Instance_OnShootingStartedAction(object sender, System.EventArgs e)
    {
        shooting = true;
    }

    public void ShakeCamera()
    {
        channelPerlin.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    private void StopShaking()
    {
        channelPerlin.m_AmplitudeGain = 0f;
        timer = 0f;
    }

    private void Update()
    {
        if (shooting) 
        {
            ShakeCamera();
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShaking();
            }
        }
    }
}
