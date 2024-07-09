using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Item : MonoBehaviour
{
    public bool Correto
    {
        get
        {
            return _correto;
        }
    }

    [SerializeField]
    private bool _correto;
    [SerializeField]
    private ItemCanvas _itemCanvas;
    [SerializeField]
    private Outline _outline;
    private XRGrabInteractable _grabInteractable;
    [SerializeField]
    private string _nome;
    [SerializeField]
    private string _informacao;
    [SerializeField]
    private float _segundosParaResetar = 5f;
    [SerializeField]
    private float _distanciaMaxima = 2f;

    private bool _isHovered = false;
    private bool _isSet = false;
    private Vector3 _posicaoInicial;
    private Quaternion _rotacaoInicial;
    private float _segundosChecagem = 1.0f;
    private float _segundosLonge;

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
        _posicaoInicial = transform.position;
        _rotacaoInicial = transform.rotation;
        InvokeRepeating(nameof(ChecarDistancia), _segundosChecagem, _segundosChecagem);
    }

    private void ChecarDistancia()
    {
        if (_isSet)
            return;

        float distancia = Vector3.Distance(transform.position, _posicaoInicial);

        if (distancia < _distanciaMaxima)
        {
            _segundosLonge = 0;
            return;
        }
        
        _segundosLonge += _segundosChecagem;
        if (_segundosLonge > _segundosParaResetar)
        {
            transform.SetPositionAndRotation(_posicaoInicial, _rotacaoInicial);
            _segundosLonge = 0;
        }
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
        if (_isSet)
            return;

        _isSet = true;
        _segundosLonge = 0;
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

    public string GetInformacao()
    {
        return $"Essa informação é sobre o item {_nome}.";
    }

}
