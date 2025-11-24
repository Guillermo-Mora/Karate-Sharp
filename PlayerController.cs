using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //Este script controla el movimiento mediante characterController del "Karateka",
    // así como sus animaciones.

    private CharacterController _characterController;
    [SerializeField] private GameObject attackGameObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _gravity = -30f;
    [SerializeField] private SuperPower superPower;
    private PlayerStats playerStats;
    private int layerRayCastRoof;
    private SpriteRenderer playerSprite;
    
    private Vector3 _hitNormal;
    private float _verticalVelocity;
    private float _rotationY;
    private BoxCollider attackHitbox;
    private bool facingLeft = false;

    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;

    void Start() {
        playerSprite = GetComponent<SpriteRenderer>();
        attackHitbox = attackGameObject.GetComponent<BoxCollider>();
        attackHitbox.enabled = false;
        playerStats = GetComponentInParent<PlayerStats>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void Move(Vector2 movementVector) {   
        Vector3 move = new Vector3(movementVector.x * _movementSpeed, 0f, 0f);
        _characterController.Move(move * Time.deltaTime);

        if (_characterController.isGrounded && _verticalVelocity < 0) {
            _verticalVelocity = -2f;
        } else {
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        _characterController.Move(new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

        _animator.SetFloat("Run", Mathf.Abs(movementVector.x));

        if (_verticalVelocity < 0 && !_characterController.isGrounded)
        {
            _animator.SetBool("Jump", false);
            _animator.SetFloat("Fall", 1f);
        }
        else if (_characterController.isGrounded)
        {
            _animator.SetFloat("Fall", -1f);
        }
        else if (_verticalVelocity >= 0)
        {
            _animator.SetFloat("Fall", -1f);
        }

        if (movementVector.x > 0) {
            playerSprite.flipX = false;
            facingLeft = false;

            Vector3 localPos = attackGameObject.transform.localPosition;
            localPos.x = Mathf.Abs(localPos.x);
            attackGameObject.transform.localPosition = localPos;
            
        } else if (movementVector.x < 0) {
            playerSprite.flipX = true;
            facingLeft = true;

            Vector3 localPos = attackGameObject.transform.localPosition;
            localPos.x = -Mathf.Abs(localPos.x);
            attackGameObject.transform.localPosition = localPos;
        }

        if (_characterController.isGrounded && _animator.GetBool("Jump"))
        {
            _animator.SetBool("Jump", false);
            _animator.SetFloat("Fall", -1);
        }
    }

    public void Jump() {
        if (_characterController.isGrounded)
        {
            _verticalVelocity = _jumpForce;
            _animator.SetBool("Jump", true);
        }
    }

    public void Fight() {
        if (_characterController.isGrounded) {
            isAttacking = true;
            Debug.Log(playerStats.SuperPowerActive);
            if (playerStats.SuperPowerActive) {
                if (attackHitbox.enabled == true) {
                    Debug.Log("Desactivo la hitbox de ataque normal si se ha quedado activada");
                    attackHitbox.enabled = false;
                }
                Debug.Log("Super poder activado");
                superPower.ThrowShuriken(facingLeft);
            } else {
                attackHitbox.enabled = true;
            }
             _animator.SetFloat("Run", 0f);
            _animator.SetBool("Fight", true);
            Invoke("endFightAnimation", 0.36f);
        }
    }

    private void endFightAnimation() {
        _animator.SetBool("Fight", false);
        attackHitbox.enabled = false;
        isAttacking = false;
    }
    
    //Player salta en trampolin
    public void TrampolineJumpPlayer(float jumpImpulse) {
        _verticalVelocity = jumpImpulse;
        _animator.SetBool("Jump", true);
        Debug.Log(_animator.GetBool("Jump"));
        Debug.Log("Trampolin detectado");
    }


    //Player recibe knockback por parte de un enemigo
    public void KnockbackPlayer(Vector3 hitDirection) {
        float force = 20f;
        float duration = 0.18f;
        float verticalForce = 20f;
        StartCoroutine(KnockbackCoroutine(hitDirection, force, duration, verticalForce));
    }


    //Para realizar esta función me he ayudado de la IA
    //
    private IEnumerator KnockbackCoroutine(Vector3 hitDirection, float force, float duration, float verticalForce)
    {
        Vector3 knockDir = new Vector3(hitDirection.x, 0f, 0f).normalized;
        float timer = 0f;

        while (timer < duration)
        {
            Vector3 knockVector = (knockDir * force + Vector3.up * verticalForce) * Time.deltaTime;
            _characterController.Move(knockVector);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}