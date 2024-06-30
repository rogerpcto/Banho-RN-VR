using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Item : MonoBehaviour
{
    [SerializeField]
    private bool _correto;
    [SerializeField]
    private ItemCanvas _itemCanvas;
    [SerializeField]
    private Outline _outline;
    private XRGrabInteractable _grabInteractable;
    [SerializeField]
    private string _nome;

    private bool _isHovered = false;
    private bool _isSet = false;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        if (_outline == null)
            _outline = GetComponent<Outline>();
        if (_grabInteractable != null)
        {
            _grabInteractable.hoverEntered.AddListener(OnHoverEntered);
            _grabInteractable.hoverExited.AddListener(OnHoverExited);
            _grabInteractable.selectEntered.AddListener(OnSelectEntered);
            _grabInteractable.selectExited.AddListener(OnSelectExited);
        }
        _itemCanvas.Inicializar(_nome);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (_isSet)
            return;

        _isHovered = true;
        SetHighlight(true);
        _itemCanvas.gameObject.SetActive(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (_isSet)
            return;

        if (_isHovered)
        {
            SetHighlight(false);
            _itemCanvas.gameObject.SetActive(false);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        _isSet = true;
        _itemCanvas.gameObject.SetActive(true);
    }
    
    private void OnSelectExited(SelectExitEventArgs args)
    {
        _isSet = false;
        _itemCanvas.gameObject.SetActive(false);
    }

    private void SetHighlight(bool highlight)
    {
        _outline.enabled = highlight;
    }

    public void SetBandejaOutline()
    {
        _isSet = true;
        _itemCanvas.gameObject.SetActive(false);
        _outline.OutlineColor = _correto ? Color.green : Color.red;
        if (_correto)
        {
            int defaultLayerMask = 1 << LayerMask.NameToLayer("Default");
            _grabInteractable.interactionLayers &= ~defaultLayerMask;
        }
        SetHighlight(true);
    }

}
