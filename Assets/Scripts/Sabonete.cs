using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sabonete : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _espumaParticleSystem;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        Eventos.InscreverSegurarBebe(() => collider.enabled = true);
        Eventos.InscreverSoltarBebe(() => collider.enabled = false);
    }

    public void CriarSabao(SelectEnterEventArgs args)
    {
        _espumaParticleSystem.transform.parent = args.interactorObject.transform;
        _espumaParticleSystem.transform.localPosition = new Vector3(-0.0045f, -0.0375f, 0.0408f);
        _espumaParticleSystem.transform.localScale = Vector3.one;
        _espumaParticleSystem.Play();
        _espumaParticleSystem.GetComponent<Collider>().enabled = true;
    }
}
