using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bebe : MonoBehaviour
{
    private Vector3 _posicaoInicial;
    private Quaternion _rotacaoInicial;
    private XRGrabInteractable _grabInteractable;
    private Animator _animator;

    [SerializeField]
    private AudioSource _choro;
    [SerializeField]
    private AudioSource _riso;
    [SerializeField]
    private bool _rostoLimpo;

    private bool _estaChorando = false;

    void Start()
    {
        _posicaoInicial = transform.position;
        _rotacaoInicial = transform.rotation;

        _animator = GetComponent<Animator>();
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.selectEntered.AddListener(Segurar);
        _grabInteractable.selectExited.AddListener(ResetPosicao);

        if (_rostoLimpo)
            LimparRosto();
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
        _animator.SetBool("Segurando", true);
    }

    private void ResetPosicao(SelectExitEventArgs args)
    {
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
}
