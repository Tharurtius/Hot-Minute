using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Singelton
    private static PlayerMovement _singleton;
    public static PlayerMovement Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (value == null)
            {
                Debug.LogWarning($"wtf you doing setting {nameof(value)}'s singleton to nothing??");
            }
            else if (value != _singleton)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the scene\nDeleting duplicate...");
                Destroy(value);
            }
        }
    }
    #endregion
    private Rigidbody2D _rigidbody2D;
    [SerializeField] public float maxMoveSpeed = 1;
    [System.NonSerialized] public float moveSpeed;
    [SerializeField] private float forwardLookDistanceMin = 0f;
    [SerializeField] private float forwardLookDistanceMax = 2f;
    [SerializeField] private float lookDistanceMax = 10f;
    [SerializeField] private bool smooth = false;
    [SerializeField] private float useCooldown = 0f;
    [SerializeField] private float cooldownMax = 1f;
    private void OnEnable()
    {
        Singleton = this;
        maxMoveSpeed *= GameManager.bonusSpeed;
    }
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        moveSpeed = maxMoveSpeed;
        cooldownMax *= GameManager.cooldownReduction;
    }
    private void FixedUpdate()
    {
        Move();
        Look();

        useCooldown -= Time.deltaTime;
    }
    private void Move()
    {
        Vector2 MoveDirection;
        if (smooth)
        {
            MoveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                MoveDirection = Vector2.zero;
            }
        }
        else
        {
            MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        MoveDirection.Normalize();
        _rigidbody2D.MovePosition(_rigidbody2D.position + MoveDirection * moveSpeed * Time.deltaTime);
    }
    private void Look()
    {
        //
        Vector2 mousePosition = GameManager.Singleton.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Mathf.Lerp(forwardLookDistanceMin, forwardLookDistanceMax, (mousePosition - (Vector2)transform.position).magnitude / lookDistanceMax);
        float angle = Vector2.SignedAngle(Vector2.right, mousePosition - (Vector2)transform.position);
        Vector3 mouseDirection = Vector3.forward * angle;
        GameManager.Singleton.mainCamera.transform.position = Vector3.LerpUnclamped(GameManager.Singleton.mainCamera.transform.position, transform.position + Quaternion.Euler(mouseDirection) * Vector3.right * distance + Vector3.back * 10f, 0.05f);
        
        transform.eulerAngles = mouseDirection;
    }
    //inventory controls
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameManager.Singleton.currentInventory.GetItem();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Singleton.currentInventory.DropItem();
        }
        else if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (useCooldown <= 0)
            {
                bool used = GameManager.Singleton.currentInventory.UseItem(transform);
                if (used)
                {
                    useCooldown = cooldownMax;
                }
            }
        }
    }
}
