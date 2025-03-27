using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event EventHandler OnDeathAction;
    public event EventHandler<OnEnemyKilledActionArgs> OnEnemyKilledAction;
    public class OnEnemyKilledActionArgs : EventArgs
    {
        public int totalKills;
    }

    [HideInInspector] public float vertical = 0;
    [HideInInspector] public float horizontal = 0;

    public float walkSpeed;
    public float runSpeed;

    public GameObject aimSp;
    //public GameObject aimSp2;
    public Transform firePoint;

    [SerializeField] private float health = 100f;

    private float maxHealth;
    private float speed;
    private int totalKills = 0;

    private Vector3 moveDir;
    private Vector3 requiredPos;

    private Rigidbody rb;

    private void Awake()
    {
        Instance = this;

        GameInput.Instance.OnSprintStartedAction += Instance_OnSprintStartedAction;
        GameInput.Instance.OnSprintCanceledAction += Instance_OnSprintCanceledAction;

        EnemyManager.Instance.OnEnemyCountChangedEvent += EnemyCountChanged;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnSprintStartedAction -= Instance_OnSprintStartedAction;
        GameInput.Instance.OnSprintCanceledAction -= Instance_OnSprintCanceledAction;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;

        speed = walkSpeed;
        maxHealth = health;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();

        //Veriables for animations
        HandleVerticalSpeed();
        HandleHorizontalSpeed();
    }

    private void HandleMovement()
    {
        moveDir = GameInput.Instance.GetMovementVector().normalized;

        rb.MovePosition(transform.position + moveDir * Time.deltaTime * speed);
    }

    private void HandleRotation()
    {
        //screenPosition = inputManager.Player.MousePosition.ReadValue<Vector2>();

        //Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Ray ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetMousePosition());
        RaycastHit rayCastHit;
        Vector3 hitPoint;

        if (Physics.Raycast(ray, out rayCastHit, float.MaxValue, LayerMask.GetMask("Ground")))
        {
            hitPoint = rayCastHit.point;

            Vector3 playerHeight = new Vector3(hitPoint.x, firePoint.position.y, hitPoint.z);

            float length = Vector3.Distance(playerHeight, hitPoint);

            var deg = 30;

            var rad = deg * Mathf.Deg2Rad;

            float hypo = length / (Mathf.Sin(rad));

            float distanceFromCamera = rayCastHit.distance;

            if (this.firePoint.position.y > hitPoint.y)
            {
                requiredPos = ray.GetPoint(distanceFromCamera - hypo);
            }
            else
            {
                requiredPos = ray.GetPoint(distanceFromCamera + hypo);
            }

            transform.LookAt(new Vector3(requiredPos.x, this.transform.position.y, requiredPos.z));
            //aimSp.transform.position = requiredPos;
            aimSp.transform.position = new Vector3(GameInput.Instance.GetMousePosition().x, GameInput.Instance.GetMousePosition().y, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            health = 0f;
            OnDeath();
        }

        GamePlayUIManager.Instance.UpdateHealthBar(health / maxHealth);
    }

    private void OnDeath()
    {
        OnDeathAction?.Invoke(this, EventArgs.Empty);
    }

    private void Instance_OnSprintCanceledAction(object sender, System.EventArgs e)
    {
        speed = walkSpeed;
    }

    private void Instance_OnSprintStartedAction(object sender, System.EventArgs e)
    {
        speed = runSpeed;
    }

    //Animation veriables functions
    private void HandleVerticalSpeed()
    {
        vertical = (Vector3.Dot(transform.forward, moveDir) * speed) / runSpeed;
    }

    private void HandleHorizontalSpeed()
    {
        horizontal = Vector3.Dot(transform.right, moveDir) * speed / runSpeed;
    }
    private void EnemyCountChanged(object sender, EnemyManager.OnEnemyCountChangedArgs e)
    {
        if (e.waveProgress != 0)
        {
            totalKills++;
        }
        Debug.Log(totalKills);
        OnEnemyKilledAction?.Invoke(this, new OnEnemyKilledActionArgs
        {
            totalKills = totalKills
        });
    }

    public Vector3 GetRequiredPosition()
    {
        return requiredPos;
    }
}
