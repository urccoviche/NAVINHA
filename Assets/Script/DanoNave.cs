using System.Collections;
using UnityEngine;

public class DanoNave : MonoBehaviour
{
    [Header("Configuração de Dano")]
    public int danoPorBatida = 1;
    public float tempoInvulneravel = 0.5f;

    [Header("Referência")]
    public GameController controller;

    private bool podeTomarDano = true;

    void Start()
    {
        if (controller == null)
        {
            controller = FindFirstObjectByType<GameController>();
        }
    }

    void OnTriggerEnter2D(Collider2D bateu)
    {
        VerificarDano(bateu.gameObject);
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        VerificarDano(colisao.gameObject);
    }

    void VerificarDano(GameObject objetoQueBateu)
    {
        if (podeTomarDano == false)
        {
            return;
        }

        if (objetoQueBateu.CompareTag("asteroid"))
        {
            if (controller != null)
            {
                controller.PerderVida(danoPorBatida);
            }

            Destroy(objetoQueBateu);

            StartCoroutine(TempoDeInvulnerabilidade());
        }
    }

    IEnumerator TempoDeInvulnerabilidade()
    {
        podeTomarDano = false;

        yield return new WaitForSeconds(tempoInvulneravel);

        podeTomarDano = true;
    }
}