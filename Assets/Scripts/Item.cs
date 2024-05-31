using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Item : MonoBehaviour
{
    [SerializeField]
    private bool _correto;
    private Outline _outline;
    private XRGrabInteractable _grabInteractable;
    private bool _isHovered = false;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _outline = GetComponent<Outline>();
        if (_grabInteractable != null)
        {
            _grabInteractable.hoverEntered.AddListener(OnHoverEntered);
            _grabInteractable.hoverExited.AddListener(OnHoverExited);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        _isHovered = true;
        SetHighlight(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (_isHovered)
        {
            SetHighlight(false);
        }
    }

    private void SetHighlight(bool highlight)
    {
        _outline.enabled = highlight;
    }

}
