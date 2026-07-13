using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

//Fuente para el codigo, explicacion y comentarios
//https://qookie.games/2d-player-movement/

public class PlayerMovement : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigid; //toma el rigidbody y hace una estancia de clase

    // Variables de velocidad de movimiento y salto
    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 19f;

    // Deteccion del terreno
    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.2f;

    // Limite de saltos
    [Header("Límites de Salto")]
    [SerializeField] private int maxJumps = 1;

    // Valores de gravedad dinámico
    [Header("Ajustes de Gravedad Dinámica")]
    [SerializeField] private float riseMultiplier = 5f;   // Multiplicador mientras sube presionando espacio
    [SerializeField] private float fallMultiplier = 5f; // Multiplicador cuando cae
    [SerializeField] private float lowJumpMultiplier = 22f; // Multiplicador si sueltas el botón rápido

    private float moveInput;
    private bool isGrounded;
    private int jumpCount;
    private bool jumpRequested; // Bandera para avisar a FixedUpdate que debe saltar


    void Start(){
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();
    }

    void Update(){
    // Movimiento horizontal
        // 1. Se captura el movimiento
        moveInput = Input.GetAxisRaw("Horizontal");

        // 1.1 Control de animaciones
        animator.SetBool("IsRunning", moveInput != 0); 
        if (moveInput < 0){
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Detectar si el jugador presiona espacio para saltar
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps){
            jumpRequested = true;
        }
    }

    void FixedUpdate() // Procesado de físicas en una función aparte para que sea independiente de los FPS
    {
        // Movimiento Horizontal
        rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y);
        
        // Verificación del suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGrounded && rigid.velocity.y <= 0.1f)
        {
            jumpCount = 0;
        }

        // Se envían los valores de aire al Animator para saltar / caer
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VelocityY", rigid.velocity.y);

        // Salto
        if (jumpRequested){
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            jumpCount++;
            jumpRequested = false;
        }

        // ---- CONTROL DE GRAVEDAD DINÁMICA ----
        // 1. Si el personaje está cayendo
        if (rigid.velocity.y < 0){
            rigid.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 2. Si está subiendo pero SUELTAS el botón (salto corto)
        else if (rigid.velocity.y > 0 && !Input.GetKey(KeyCode.Space)){
            rigid.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 3. Si está subiendo y se mantiene presionado el botón (salto largo pero rápido)
        else if (rigid.velocity.y > 0 && Input.GetKey(KeyCode.Space)){
            rigid.velocity += Vector2.up * Physics2D.gravity.y * (riseMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // Dibuja la esfera en el editor para que puedas calibrar visualmente el tamaño del GroundCheck
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }


}