using System.Linq;
using UnityEngine;

public class DirtRemover : MonoBehaviour
{
    private const string MATERIAL = "Std_Skin_Head";
    private const float CLEANING_RATE = .5f;
    private Vector3 _lastPosition;
    private bool _isCleaning = false;

    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private string _tag;

    private void OnTriggerEnter(Collider other)
    {
        var algodao = other.GetComponent<Algodao>();
        if (algodao != null && algodao.IsWet)
        {
            _isCleaning = true;
            _lastPosition = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var algodao = other.GetComponent<Algodao>();
        if (algodao != null)
        {
            _isCleaning = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var algodao = other.GetComponent<Algodao>();
        if (algodao != null && _isCleaning)
        {
            Vector3 currentPosition = other.transform.position;
            float distanceMoved = Vector3.Distance(currentPosition, _lastPosition);
            _lastPosition = currentPosition;

            var materials = _renderer.materials;
            var material = materials.First(m => m.name.Contains(MATERIAL));

            float currentOpacity = material.GetFloat($"_Dirt{_tag}Opacity");
            float newOpacity = Mathf.Clamp(currentOpacity - (distanceMoved * CLEANING_RATE), 0f, 1f);
            material.SetFloat($"_Dirt{_tag}Opacity", newOpacity);
        }
    }
}
