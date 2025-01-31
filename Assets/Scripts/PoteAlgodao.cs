using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PoteAlgodao : MonoBehaviour
{
    [SerializeField]
    private GameObject _algodaoPrefab;
    [SerializeField]
    private Transform _spawnPoint;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        Eventos.InscreverSegurarBebe(() => collider.enabled = true);
        Eventos.InscreverSoltarBebe(() => collider.enabled = false);
    }

    public void CriarAlgodao(SelectEnterEventArgs args)
    {
        var currentInteractor = args.interactorObject as XRBaseInteractor;

        if (currentInteractor == null || _algodaoPrefab == null || _spawnPoint == null)
            return;

        GameObject novoAlgodao = Instantiate(_algodaoPrefab, _spawnPoint.position, _spawnPoint.rotation);

        var interactable = novoAlgodao.GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            currentInteractor.interactionManager.SelectEnter(currentInteractor as IXRSelectInteractor, interactable);
        }
    }
}
