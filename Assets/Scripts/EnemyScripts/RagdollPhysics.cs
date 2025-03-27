using UnityEngine;

public class RagdollPhysics : MonoBehaviour
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

    public void RagdollOn()
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
