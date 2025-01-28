using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Agua : MonoBehaviour
{
    [SerializeField]
    private ItemCanvas _canvas;
    [SerializeField]
    private Slider _slider;

    private ActionBasedController _controller;
    private ParticleSystem _particleSystem;
    private float _qtde;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_controller == null)
            return;

        if (!_controller.selectAction.action.IsPressed())
        {
            if (_qtde <= 0 && _particleSystem.isPlaying)
            {
                _particleSystem.Stop();
                _controller = null;
                return;
            }

            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }

            _qtde = Mathf.Max(_qtde - 0.5f, 0);
            _slider.value = _qtde / 10;
        }
        else if (_particleSystem.isPlaying)
        {
            _particleSystem.Stop();
        }
    }

    public void PegarAgua(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject as XRBaseControllerInteractor;
        if (interactor != null)
        {
            // HACK: Solução alternativa para não bloquear o raio na banheira
            interactor.enabled = false;
            interactor.enabled = true;
            _controller = interactor.xrController as ActionBasedController;
        }

        _particleSystem.transform.parent = args.interactorObject.transform;
        _particleSystem.transform.localPosition = new Vector3(-0.0045f, -0.0375f, 0.0408f);

        _qtde = 100;

        _canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _canvas.Inicializar("Água");
        _canvas.gameObject.SetActive(true);
        _slider.value = _qtde / 10;
    }
}
