using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject rig;
    
    private Rigidbody[] ragdollRigidbodies;
    private CapsuleCollider[] ragdollCapsuleColliders;
    private void Start()
    {
        GetColliders();
        GetRigidBodies();
        RagdollOff();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Dino");
        GetComponent<Rigidbody>().AddForce(Vector3.back * 10f, ForceMode.Impulse);
        RagdollOn();
    }

    private void RagdollOff()
    {
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (var collider in ragdollCapsuleColliders)
        {
            collider.enabled = false;
        }
    }

    private void RagdollOn()
    {
        GetComponent<Animator>().enabled = false;
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (var collider in ragdollCapsuleColliders)
        {
            collider.enabled = true;
        }
    }

    private void GetColliders()
    {
        ragdollCapsuleColliders = rig.GetComponentsInChildren<CapsuleCollider>();
    }

    private void GetRigidBodies()
    {
        ragdollRigidbodies = rig.GetComponentsInChildren<Rigidbody>();
    }
}
