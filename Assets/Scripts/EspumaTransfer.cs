using UnityEngine;

public class EspumaTransfer : MonoBehaviour
{
    public const int ESPUMA_MAX = 40;
    private const float TAXA_LIMPEZA = 5f;

    private Vector3 _ultimaPosicao;
    private bool _estaEnsaboado;

    [SerializeField]
    private ParticleSystem _espumaParticleSystem;
    [SerializeField]
    private Bebe _bebe;

    private void OnTriggerStay(Collider other)
    {
        if (!_estaEnsaboado && other.CompareTag("Espuma"))
        {
            Vector3 posicaoAtual = other.transform.position;
            float distanciaMovida = Vector3.Distance(posicaoAtual, _ultimaPosicao);
            _ultimaPosicao = posicaoAtual;

            var emission = _espumaParticleSystem.emission;
            var rate = emission.rateOverTime;

            float taxaAtual = rate.constant;
            float taxaNova = Mathf.Clamp(taxaAtual + (distanciaMovida * TAXA_LIMPEZA), 0f, ESPUMA_MAX);

            rate.constant = taxaNova;
            emission.rateOverTime = rate;

            if (taxaNova >= ESPUMA_MAX)
            {
                _estaEnsaboado = true;
                _bebe.Sorrir();
                Eventos.InvocarComecarEnxague();
            }
        }
    }
}