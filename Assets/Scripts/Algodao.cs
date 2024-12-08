using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Algodao : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private bool _isHold;

    [SerializeField]
    private float _destroyDelay = 2.0f;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener((args) =>
            {
                Invoke(nameof(SelfDestroy), _destroyDelay);
            });
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveAllListeners();
        }
    }
}