using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Algodao : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private Renderer _renderer;
    private bool _isHold;
    private bool _isWet;

    public bool IsWet { get => _isWet; }

    [SerializeField]
    private float _destroyDelay = 2.0f;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _renderer = GetComponent<Renderer>();

        if (_grabInteractable != null)
        {
            _grabInteractable.selectExited.AddListener((args) =>
            {
                Invoke(nameof(SelfDestroy), _destroyDelay);
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agua") && !_isWet)
        {
            BecomeWet();
        }
    }

    private void BecomeWet()
    {
        _isWet = true;
        SetBaseColor(new Color(120 / 255f, 140 / 255f, 160 / 255f));
    }

    private void SetBaseColor(Color color) => _renderer.material.color = color;

    private void SelfDestroy() => Destroy(gameObject);

    private void OnDestroy()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectExited.RemoveAllListeners();
        }
    }
}