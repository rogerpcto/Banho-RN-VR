using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    private ActionBasedController _controller;
    [SerializeField]
    private Hand _hand;

    private void Start()
    {
        _controller = GetComponentInParent<ActionBasedController>();
    }

    private void Update()
    {
        _hand.SetGrip(_controller.selectAction.action.ReadValue<float>());
        _hand.SetTrigger(_controller.activateAction.action.ReadValue<float>());
    }
}
