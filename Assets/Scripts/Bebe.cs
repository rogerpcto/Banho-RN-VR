using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bebe : MonoBehaviour
{
    private Vector3 _posicaoInicial;
    private Quaternion _rotacaoInicial;
    private XRGrabInteractable _grabInteractable;

    void Start()
    {
        _posicaoInicial = transform.position;
        _rotacaoInicial = transform.rotation;

        _grabInteractable = GetComponent<XRGrabInteractable>();

        _grabInteractable.selectExited.AddListener(ResetPosicao);
    }

    private void ResetPosicao(SelectExitEventArgs args)
    {
        transform.position = _posicaoInicial;
        transform.rotation = _rotacaoInicial;
    }
}
