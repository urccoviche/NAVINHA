using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    [Range(0, 10)]
    public float velocidade = 5f;

    private Rigidbody2D rig;

    [Header("Câmera")]
    public Camera cameraDoJogo;

    [Header("Tiro")]
    public GameObject laser;
    public Transform disparo;
    public AudioSource somLazer;
    public GameController controller;

    // Classe gerada pelo Input System
    private InputSystem_Actions inputActions;

    // Guarda a direção lida no Update
    private Vector2 direcao;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        somLazer = GetComponent<AudioSource>();

        if (cameraDoJogo == null)
        {
            cameraDoJogo = Camera.main;
        }

        // Cria uma instância da classe gerada
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        // Habilita o Action Map Player
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        // Desabilita quando o objeto for desligado
        inputActions.Player.Disable();
    }

    void Update()
    {
        // Lê o movimento no Update
        direcao = inputActions.Player.Move.ReadValue<Vector2>();

        GirarParaMouse();
        Disparar();
    }

    void FixedUpdate()
    {
        // Movimento físico fica no FixedUpdate
        Mover();
    }

    void Mover()
    {
        rig.linearVelocity = direcao * velocidade;
    }

    void GirarParaMouse()
    {
        // Evita erro caso o jogo esteja sem mouse detectado
        if (Mouse.current == null || cameraDoJogo == null)
            return;

        Vector2 posicaoMouse = cameraDoJogo.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Vector2 distancia = posicaoMouse - rig.position;

        float anguloMira = Mathf.Atan2(distancia.y, distancia.x) * Mathf.Rad2Deg;

        // Melhor para Rigidbody2D do que alterar rig.rotation direto
        rig.SetRotation(anguloMira);
    }

    void Disparar()
    {
        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            Instantiate(laser, disparo.position, disparo.rotation);

            if (somLazer != null)
            {
                somLazer.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D bateu)
    {
        if (bateu.gameObject.CompareTag("x"))
        {
            transform.position = new Vector3(
                transform.position.x * -0.9f,
                transform.position.y,
                transform.position.z
            );
        }

        if (bateu.gameObject.CompareTag("y"))
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y * -0.9f,
                transform.position.z
            );
        }
    }
}