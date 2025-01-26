using System.Linq;
using UnityEngine;

public class DirtRemover : MonoBehaviour
{
    private const string MATERIAL = "Std_Skin_Head";
    private const float TAXA_LIMPEZA = .5f;
    private Vector3 _ultimaPosicao;
    private bool _estaLimpando = false;

    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private string _tag;

    private void OnTriggerEnter(Collider other)
    {
        var algodao = other.GetComponent<Algodao>();
        if (algodao != null && algodao.IsWet)
        {
            _estaLimpando = true;
            _ultimaPosicao = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var algodao = other.GetComponent<Algodao>();
        if (algodao != null)
        {
            _estaLimpando = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var algodao = other.GetComponent<Algodao>();
        if (algodao != null && _estaLimpando)
        {
            Vector3 posicaoAtual = other.transform.position;
            float distanciaMovida = Vector3.Distance(posicaoAtual, _ultimaPosicao);
            _ultimaPosicao = posicaoAtual;

            var materials = _renderer.materials;
            var material = materials.First(m => m.name.Contains(MATERIAL));

            float opacidadeAtual = material.GetFloat($"_Dirt{_tag}Opacity");
            float opacidadeNova = Mathf.Clamp(opacidadeAtual - (distanciaMovida * TAXA_LIMPEZA), 0f, 1f);
            material.SetFloat($"_Dirt{_tag}Opacity", opacidadeNova);
        }
    }

    public void Limpar()
    {
        var materials = _renderer.materials;
        var material = materials.First(m => m.name.Contains(MATERIAL));

        float currentOpacity = material.GetFloat($"_Dirt{_tag}Opacity");
        material.SetFloat($"_Dirt{_tag}Opacity", 0);
    }
}
