using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bebe : MonoBehaviour
{
    private Vector3 _posicaoInicial;
    private Quaternion _rotacaoInicial;
    private XRGrabInteractable _grabInteractable;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _choroBebe;

    private bool _estaChorando = false;

    void Start()
    {
        _posicaoInicial = transform.position;
        _rotacaoInicial = transform.rotation;

        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.selectExited.AddListener(ResetPosicao);
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
                _audioSource.PlayOneShot(_choroBebe);
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
        _audioSource.Stop();
        _estaChorando = false;
    }

    private void ResetPosicao(SelectExitEventArgs args)
    {
        transform.position = _posicaoInicial;
        transform.rotation = _rotacaoInicial;
    }
}
