using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraFollower : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 miniCamPos;

    private void Start()
    {
        miniCamPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, miniCamPos.y, player.transform.position.z);
    }
}
