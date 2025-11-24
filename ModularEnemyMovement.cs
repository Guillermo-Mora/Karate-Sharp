using UnityEngine;
using System.Collections;

public class ModularEnemyMovement : MonoBehaviour
{
    //Script que tienen todos los enemigos que se mueven.

    //He usado characterController ya que siento que me permitía añadir mas funcionalidades
    // y comportamientos diferentes y modificables a los enemigos.
    //Mediante este script, desde el inspector puedo regular su velocidad, salto, dirección de movimiento, etc
    // a medida para cada enemigo que quiera añadir.

    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 9.5f;
    [SerializeField] private float jumpForce = 38f;
    [SerializeField] private float gravity = -100f;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private bool followsTargetOnEnterZone = true;
    [SerializeField] private bool followsTarget = true;
    [SerializeField] private bool followsDirectionNotTarget = false;
    [SerializeField] private string followDirection = "";
    [SerializeField] private bool isAlwaysJumping = false;
    [SerializeField] private bool jumpsOnWallsDetected = true;
    [SerializeField] private bool changesDirectionOnWallsDetected = false;
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private SpriteRenderer spriteRenderer;
    private Vector3 velocity;
    private Vector3 rayOrigin;
    private Vector3 direction;
    private float wallCooldownDuration = 0.01f;
    private bool wallCooldownActive = false;
    private float wallCooldownTimer = 0f;
    private bool died = false;
    private BoxCollider hitBox;

    private void Start()
    {
        hitBox = GetComponentInChildren<BoxCollider>();
        controller = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Para algunnos enemigos he tenido que poner el sprite a parte para regular su tamaño,
        // por lo que si no lo encuentra, es porque se encuentra un nivel mas abajo.
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (died) return;

        if (followsTarget) {
            direction = (target.position - transform.position).normalized;
        }

        if (followsDirectionNotTarget) {
            if (followDirection == "left") direction = Vector3.left;
            else if (followDirection == "right") direction = Vector3.right; 
        }

        direction.y = 0f;

        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;

        if (isAlwaysJumping) {
            if (controller.isGrounded) velocity.y = jumpForce;
        }

        if (jumpsOnWallsDetected || changesDirectionOnWallsDetected){
            rayOrigin = transform.position + controller.center - new Vector3(0, 1f, 0);
             if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, wallCheckDistance)) {
                if (hit.collider && !hit.collider.CompareTag("Character") && controller.isGrounded) {

                    if (jumpsOnWallsDetected) velocity.y = jumpForce;

                    if (changesDirectionOnWallsDetected) OnWallDetected();
                }
            }
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 move = direction * moveSpeed;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

        if (direction.x != 0) spriteRenderer.flipX = direction.x < 0;
    }

    private bool CanChangeDirection()
    {
        if (!wallCooldownActive) return true;
        wallCooldownTimer += Time.deltaTime;

        if (wallCooldownTimer >= wallCooldownDuration)
        {
            wallCooldownActive = false;
            wallCooldownTimer = 0f;
            return true;
        }
        return false;
    }

    //He tenido que implementar un cooldown en la detección de paredes.
    //Ya que al detectar paredes y cambiar de dirección, aún siguen
    //en contacto con la pared, entonces comenzaban a girar sobre si mismo sin parar.
    private void OnWallDetected()
    {
        if (CanChangeDirection())
        {
            direction = (followDirection == "left") ? Vector3.right : Vector3.left;
            followDirection = (followDirection == "left") ? "right" : "left";
            wallCooldownActive = true;
            wallCooldownTimer = 0f;
        }
    }

    public void DieSequence() {
        Debug.Log("Entra animacion de muerte del enemigo");
        died = true;
        hitBox.enabled = false;
        spriteRenderer.flipY = true;
        StartCoroutine(DeathMotion());
    }

    //Para realizar esta función me he ayudado de la IA
    //
    private IEnumerator DeathMotion() {
    Vector3 throwDir = new Vector3(direction.x * -4f, 20f, -40f);
    float gravity = -50f;
    float timer = 0f;
    float duration = 1f;

    while (timer < duration) {
        throwDir.y += gravity * Time.deltaTime;
        controller.Move(throwDir * Time.deltaTime);
        timer += Time.deltaTime;
        yield return null;
    }

    Destroy(transform.parent.gameObject);
    Debug.Log("Destruyo el GO del enemigo");
    }

    public void ChaseTarget() {
        animator.SetBool("run", true);
        if (followsTargetOnEnterZone) followsTarget = true;
    }

}