using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Asteroide")]
    public Transform[] pontoOrigem;
    public GameObject asteroid;
    public float timer;
    public float intervaloTempo = 2f;

    [Header("Pontuação")]
    public int pontos;
    public TextMeshProUGUI txtPontos;

    [Header("Sistema de Vida")]
    public int vidaMaxima = 5;
    public int vidaAtual;
    public Slider sliderVida;

    [Header("Tela de Game Over")]
    public GameObject telaGameOver;

    private bool jogoAcabou = false;

    void Start()
    {
        Time.timeScale = 1f;

        timer = intervaloTempo;

        vidaAtual = vidaMaxima;

        AtualizarSliderVida();
        AtualizarTextoPontos();

        if (telaGameOver != null)
        {
            telaGameOver.SetActive(false);
        }
    }

    void Update()
    {
        if (jogoAcabou == false)
        {
            CriarAsteroides();
        }
    }

    void CriarAsteroides()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (pontoOrigem.Length > 0 && asteroid != null)
            {
                // Correção importante:
                // Random.Range com int já exclui o último valor automaticamente.
                int pontoAleatorio = Random.Range(0, pontoOrigem.Length);

                Instantiate(
                    asteroid,
                    pontoOrigem[pontoAleatorio].position,
                    pontoOrigem[pontoAleatorio].rotation
                );
            }

            timer = intervaloTempo;
        }
    }

    public void recebePontos(int recebe)
    {
        if (jogoAcabou == true)
        {
            return;
        }

        pontos += recebe;
        AtualizarTextoPontos();
    }

    public void PerderVida(int dano)
    {
        if (jogoAcabou == true)
        {
            return;
        }

        vidaAtual -= dano;

        if (vidaAtual < 0)
        {
            vidaAtual = 0;
        }

        AtualizarSliderVida();

        if (vidaAtual <= 0)
        {
            GameOver();
        }
    }

    void AtualizarSliderVida()
    {
        if (sliderVida != null)
        {
            sliderVida.maxValue = vidaMaxima;
            sliderVida.value = vidaAtual;
        }
    }

    void AtualizarTextoPontos()
    {
        if (txtPontos != null)
        {
            txtPontos.text = "Pontos: " + pontos;
        }
    }

    void GameOver()
    {
        jogoAcabou = true;

        if (telaGameOver != null)
        {
            telaGameOver.SetActive(true);
        }

        // Pausa o jogo
        Time.timeScale = 0f;
    }

    public void ReiniciarPartida()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}