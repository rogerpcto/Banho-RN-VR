using TMPro;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private Transform cameraTransform;

    public void Inicializar(string nome)
    {
        cameraTransform = Camera.main.transform;
        text.text = nome;
        
    }

    void Update()
    {
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
