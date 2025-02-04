using UnityEngine;

public class EspumaRemover : MonoBehaviour
{
    private const float TAXA_REMOCAO = 0.1f;

    [SerializeField]
    private ParticleSystem _espumaParticleSystem;
    [SerializeField]
    private Bebe _bebe;

    private void OnParticleCollision(GameObject other)
    {
        var emission = _espumaParticleSystem.emission;
        var rate = emission.rateOverTime;

        if (rate.constant == EspumaTransfer.ESPUMA_MAX)
            Eventos.InvocarLiberarFinalizarFase3();

        if (rate.constant <= 0)
            return;

        float taxaAtual = rate.constant;
        float taxaNova = Mathf.Clamp(taxaAtual - TAXA_REMOCAO, 0f, float.MaxValue);

        rate.constant = taxaNova;
        emission.rateOverTime = rate;

        if (taxaNova <= 0)
        {
            _bebe.Sorrir();
            _bebe.RemoverSabao();
        }
    }
}