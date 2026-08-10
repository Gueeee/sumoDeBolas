using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BolinhaController : MonoBehaviour {
    public Rigidbody rb;
    public float mvspd = 5f;
    public float maxSpeed;
    public float pushStrength = 0f;
    public float maxPushStrength = 20f;
    private Vector2 _moveDir;

    public int playerId = 0;
    public int coins;

    [SerializeField] private BolinhaData bolinhaData;
    private MeshRenderer meshRenderer;

    public PlayerInput playerInput;
    private InputAction move;
    private InputAction attack;

    public static event Action<int, Sprite> OnUpdatePortrait;
    public static event Action<int, int> OnCoinCollect;
    public static event Action<int> OnDefeat;

    public Sprite playerSprite;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();

        if(playerId == 0) bolinhaData = GameManager.Instance?.P1;
        else bolinhaData = GameManager.Instance?.P2;

        if (playerInput.currentActionMap.name == "Player1") {
            move = playerInput.actions.FindAction("Move1");
            attack = playerInput.actions.FindAction("Attack1");
        } else if (playerInput.currentActionMap.name == "Player2") {
            move = playerInput.actions.FindAction("Move2");
            attack = playerInput.actions.FindAction("Attack2");
        }
        
        maxSpeed = bolinhaData.speed;
        maxPushStrength = bolinhaData.pushStrength;
        meshRenderer = GetComponent<MeshRenderer>();

        playerSprite = bolinhaData.sprite;
        Texture2D spriteTexture = playerSprite.texture;
        
        meshRenderer.material.mainTexture = spriteTexture;
    }

    void OnEnable() {
        attack?.Enable();
        BolinhaController.OnDefeat += StopMovement;

        if(attack != null) {
            attack.started += Attack;
        }
    }

    void OnDisable() {
        attack?.Disable();
        BolinhaController.OnDefeat -= StopMovement;

        if(attack != null) {
            attack.started -= Attack;
        }
    }

    private void StartBolinha(BolinhaData p1, BolinhaData p2) {
        if(playerId == 0) {
            bolinhaData = p1;
        } else {
            bolinhaData = p2;
        }
    }

    private void Attack(InputAction.CallbackContext context) {
        AttackAction();
    }

    private void StopMovement(int playerId) {
        mvspd = 0;
        rb.linearVelocity = new Vector3(0f, 0f, 0f);
    }

    void Start() {
        OnUpdatePortrait?.Invoke(playerId, playerSprite);
        
        OnCoinCollect?.Invoke(playerId, coins);
    }

    void Update() {
        _moveDir = move.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Movement();

        // rb.linearVelocity = new Vector3(_moveDir.x * mvspd,0, _moveDir.y * mvspd);
    }

    private void AttackAction() {
        StartCoroutine(OnAttack());
    }

    public void Movement() {
        if (!rb) return;

        Vector3 desired = new Vector3(_moveDir.x, 0f, _moveDir.y);

        if (desired.sqrMagnitude > 1f) desired.Normalize();

        Vector3 force = desired * mvspd;
        rb.AddForce(force, ForceMode.Force);
        
        if (maxSpeed > 0f) {
            Vector3 horizontalVel = rb.linearVelocity;
            horizontalVel.y = 0f;
            float speed = horizontalVel.magnitude;
            if (speed > maxSpeed) {
                Vector3 limited = horizontalVel.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Deadzone") {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y+100f, 0f);
            
            OnDefeat?.Invoke(playerId);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Coin") {
            coins++;
            mvspd -= (mvspd*0.025f)*coins;

            OnCoinCollect?.Invoke(playerId, coins);
            
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Player") {
            Rigidbody _rb = other.GetComponent<Rigidbody>();
            
            if (_rb != null) {
                Vector3 pushDirection = _rb.position - transform.position;
                pushDirection.y = 0;
                pushDirection = pushDirection.normalized;
                _rb.AddForce(pushDirection * pushStrength, ForceMode.Impulse);
            }
        }
    }

    IEnumerator OnAttack() {
        Debug.Log("Ataque!");
        pushStrength = maxPushStrength+((maxPushStrength*0.025f)*coins);

        yield return new WaitForSeconds(.5f);

        pushStrength = 0;
    }
}