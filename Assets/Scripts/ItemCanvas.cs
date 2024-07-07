using TMPro;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private Transform cameraTransform;
    private Vector3 offset;


    public void Inicializar(string nome)
    {
        cameraTransform = Camera.main.transform;
        text.text = nome;
        offset = transform.position - transform.parent.position;
    }

    void LateUpdate()
    {
        transform.position = transform.parent.position + offset;

        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
