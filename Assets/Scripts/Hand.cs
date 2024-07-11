using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Hand : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Animator _animator;
    private float _gripTarget;
    private float _triggerTarget;
    private float _gripCurrent;
    private float _triggerCurrent;

    private const string _animatorGripParam = "Grip";
    private const string _animatorTriggerParam = "Trigger";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateHand();
    }

    public void SetGrip(float value)
    {
        _gripTarget = value;
    }

    public void SetTrigger(float value)
    {
        _triggerTarget = value;
    }

    private void AnimateHand()
    {
        if (_gripCurrent != _gripTarget)
        {
            _gripCurrent = Mathf.MoveTowards(_gripCurrent, _gripTarget, Time.deltaTime * _speed);
            _animator.SetFloat(_animatorGripParam, _gripCurrent);
        }
        if (_triggerCurrent != _triggerTarget)
        {
            _triggerCurrent = Mathf.MoveTowards(_triggerCurrent, _triggerTarget, Time.deltaTime * _speed);
            _animator.SetFloat(_animatorTriggerParam, _triggerCurrent);
        }
    }
}
