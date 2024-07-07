using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BandejaSocket : MonoBehaviour
{
    [SerializeField]
    private Bandeja _bandeja; 
    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnSelectEntered);
            socketInteractor.selectExited.AddListener(OnSelectExited);
        }
    }

    private void OnDestroy()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        XRGrabInteractable interactable = args.interactableObject as XRGrabInteractable;
        Item item = interactable.gameObject.GetComponent<Item>();
        if (item != null)
        {
            item.SetBandejaOutline();
            _bandeja.CadastrarItem(item);
        }
    }
    private void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("Saiu");
    }
}
