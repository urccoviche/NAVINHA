using UnityEngine;
using UnityEngine.InputSystem; // ← namespace novo obrigatório!

public class Player : MonoBehaviour
{
    [Range(0, 10)] public int velocidade;
    Rigidbody2D rig;

    // Referência à câmera (igual antes)
    public Camera cameraDoJogo;

    // Variáveis do tiro (igual antes)
    public GameObject laser;
    public Transform disparo;
    public AudioSource somLazer;
    public GameController controller;

    // ===== NOVO INPUT SYSTEM =====
    // Criamos variáveis do tipo InputAction para cada ação
    private InputAction moverAction;    // vai substituir Input.GetAxis
    private InputAction atirarAction;   // vai substituir Input.GetButtonDown

    void Awake()
    {
        // Aqui conectamos nossas variáveis às ações que existem
        // no InputSystem_Actions (aquele arquivo criado no passo 3)
        // "Player/Move" = Action Map "Player", ação "Move"
        moverAction = InputSystem.actions.FindAction("Player/Move");
        atirarAction = InputSystem.actions.FindAction("Player/Attack");
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        somLazer = GetComponent<AudioSource>();
    }

    void Update()
    {
        mover();
        disparar();
    }

    void mover()
    {
        // ANTES: Input.GetAxis("Horizontal") e Input.GetAxis("Vertical")
        // AGORA: lemos um Vector2 direto da ação "Move"
        Vector2 direcao = moverAction.ReadValue<Vector2>();
        rig.linearVelocity = direcao * velocidade;

        // ANTES: Input.mousePosition
        // AGORA: Mouse.current.position (precisa do using UnityEngine.InputSystem)
        Vector2 posicaoMouse = cameraDoJogo.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Vector2 distancia = posicaoMouse - rig.position;
        float anguloMira = Mathf.Atan2(distancia.y, distancia.x) * Mathf.Rad2Deg;
        rig.rotation = anguloMira;
    }

    void disparar()
    {
        // ANTES: Input.GetButtonDown("Fire1")
        // AGORA: WasPressedThisFrame() — verifica se apertou neste frame
        if (atirarAction.WasPressedThisFrame())
        {
            Instantiate(laser, disparo.position, disparo.rotation);
            somLazer.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D bateu)
    {
        // Esse trecho NÃO muda! Colisão não tem nada a ver com input
        if (bateu.gameObject.tag == "x")
        {
            transform.position = new Vector3(
                transform.position.x * -0.9f,
                transform.position.y,
                transform.position.z
            );
        }
        if (bateu.gameObject.tag == "y")
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y * -0.9f,
                transform.position.z
            );
        }
    }
}