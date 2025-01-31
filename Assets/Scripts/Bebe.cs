using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bebe : MonoBehaviour
{
    private List<string> _tags = new List<string> { "Testa", "Bochecha Direita", "Bochecha Esquerda", "Queixo" };

    private Vector3 _posicaoInicial;
    private Quaternion _rotacaoInicial;
    private XRGrabInteractable _grabInteractable;
    private Animator _animator;

    [SerializeField]
    private AudioSource _choro;
    [SerializeField]
    private AudioSource _riso;

    private bool _estaChorando = false;

    void Start()
    {
        _posicaoInicial = transform.position;
        _rotacaoInicial = transform.rotation;

        _animator = GetComponent<Animator>();
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.selectEntered.AddListener(Segurar);
        _grabInteractable.selectExited.AddListener(ResetPosicao);

        Eventos.InscreverComecarFase2(() => GetComponent<Collider>().enabled = true);
        Eventos.InscreverComecarFase3(LimparRosto);
        Eventos.InscreverTerminarFase2(GerarMensagemFase2);
    }

    private void Update()
    {
        CheckOrientacao();
    }

    private void CheckOrientacao()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0.7f)
        {
            if (!_estaChorando)
            {
                _estaChorando = true;
                _choro.Play();
            }
        }
        else
        {
            StartCoroutine(PararChoro());
        }
    }

    private IEnumerator PararChoro()
    {
        yield return new WaitForSeconds(.5f);
        _choro.Stop();
        _estaChorando = false;
    }

    private void Segurar(SelectEnterEventArgs args)
    {
        Eventos.InvocarSegurarBebe();
        _animator.SetBool("Segurando", true);
    }

    private void ResetPosicao(SelectExitEventArgs args)
    {
        Eventos.InvocarSoltarBebe();
        _animator.SetBool("Segurando", false);
        transform.SetPositionAndRotation(_posicaoInicial, _rotacaoInicial);
    }

    public void Sorrir()
    {
        _riso.Play();
        _animator.SetTrigger("Sorriso");
    }

    private void LimparRosto()
    {
        var dirtRemovers = GetComponentsInChildren<DirtRemover>();
        foreach (var dirtRemover in dirtRemovers)
        {
            dirtRemover.Limpar();
            Destroy(dirtRemover.gameObject);
        }
    }

    public void ZonaLimpa(string tag)
    {
        Eventos.InvocarLiberarFinalizarFase2();
        _tags.Remove(tag);
    }

    private void GerarMensagemFase2()
    {
        string mensagem = "Parabéns! Você terminou a limpeza do rosto do bebê!";
        if (_tags.Count != 0)
        {
            mensagem += " Mas faltou limpar as seguintes áreas: ";
            foreach (var tag in _tags)
            {
                mensagem += tag + ", ";
            }
            mensagem = mensagem.Remove(mensagem.Length - 2);
        }
        Eventos.InvocarGerarMensagemFase2(mensagem);
    }
}
