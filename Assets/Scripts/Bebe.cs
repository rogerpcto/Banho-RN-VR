using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bebe : MonoBehaviour
{
    private const string MENSAGEM_FORA_DE_POSICAO = " Lembre-se de sempre manter o beb� de barriga para cima, prezando pela sua seguran�a!";
    private const float TEMPO_MAX_FORA_DE_POSICAO = 30f;

    [SerializeField]
    private AudioSource _choro;
    [SerializeField]
    private AudioSource _riso;
    [SerializeField]
    private Collider _cabeca;

    private List<string> _tags = new List<string> { "Testa", "Bochecha Direita", "Bochecha Esquerda", "Queixo" };
    private Vector3 _posicaoInicial;
    private Quaternion _rotacaoInicial;
    private XRGrabInteractable _grabInteractable;
    private Animator _animator;


    private bool _estaChorando = false;
    private bool _estaComSabao = true;
    private float _timerForaDePosicao;

    void Start()
    {
        _posicaoInicial = transform.position;
        _rotacaoInicial = transform.rotation;

        _animator = GetComponent<Animator>();
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.selectEntered.AddListener(Segurar);
        _grabInteractable.selectExited.AddListener(ResetPosicao);

        Eventos.InscreverComecarFase2(() => GetComponent<Collider>().enabled = true);
        Eventos.InscreverTerminarFase2(() =>
        {
            GetComponent<Collider>().enabled = false;
            Eventos.LimparEventosSegurarBebe();
            GerarMensagemFase2();
        });
        Eventos.InscreverComecarFase3(() =>
        {
            GetComponent<Collider>().enabled = true;
            _cabeca.enabled = true;
            LimparRosto();
        });
        Eventos.InscreverTerminarFase3(() =>
        {
            GetComponent<Collider>().enabled = false;
            _cabeca.enabled = false;
            Eventos.LimparEventosSegurarBebe();
            GerarMensagemFase3();
        });
    }

    private void Update()
    {
        CheckOrientacao();
    }

    private void CheckOrientacao()
    {
        float sinal = Mathf.Sign(Vector3.Dot(transform.forward, Vector3.down));
        float angulo = sinal * Vector3.Angle(transform.up, Vector3.up);

        if (angulo > 30f || angulo < 0)
        {
            _timerForaDePosicao += Time.deltaTime;
        }

        if (angulo > 45f || angulo < -30f)
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
        string mensagem = "Parab�ns! Voc� terminou a limpeza do rosto do beb�!";
        if (_tags.Count != 0)
        {
            mensagem += " Mas faltou limpar as seguintes �reas: ";
            foreach (var tag in _tags)
            {
                mensagem += tag + ", ";
            }
            mensagem = mensagem.Remove(mensagem.Length - 2);
            mensagem += ".";
        }

        if (_timerForaDePosicao > TEMPO_MAX_FORA_DE_POSICAO)
            mensagem += MENSAGEM_FORA_DE_POSICAO;

        Eventos.InvocarGerarMensagemFase2(mensagem);
    }

    public void RemoverSabao() => _estaComSabao = false;
    public void GerarMensagemFase3()
    {
        string mensagem = "Parab�ns! Voc� terminou a limpeza da cabe�a do beb�!";

        if (_estaComSabao)
            mensagem += " Mas n�o se esque�a de remover todo o sab�o do beb�.";

        if (_timerForaDePosicao > TEMPO_MAX_FORA_DE_POSICAO)
            mensagem += MENSAGEM_FORA_DE_POSICAO;

        Eventos.InvocarGerarMensagemFase3(mensagem);
    }
}
