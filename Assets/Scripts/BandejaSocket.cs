using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BandejaSocket : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerToRemove;
    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnDestroy()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        XRGrabInteractable interactable = args.interactableObject as XRGrabInteractable;
        Item item = interactable.gameObject.GetComponent<Item>();
        if (item != null)
        {
            item.SetBandejaOutline();
        }
    }
}
