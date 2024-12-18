using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuExpandButton : MonoBehaviour
{
    [SerializeField]
    private Transform[] _childButtons;
    [SerializeField]
    private float _expandDuration = 0.3f;
    private AnimationCurve _expandCurve = new AnimationCurve(
        new Keyframe(0, 0, 0, 2),
        new Keyframe(0.5f, 0.8f, 1, 1),
        new Keyframe(1, 1, 2, 0)
    );

    private bool isExpanded = false;

    void Start()
    {
        foreach (Transform button in _childButtons)
        {
            button.localScale = Vector3.zero;
            button.gameObject.SetActive(false);
        }
        GetComponent<Button>().onClick.AddListener(ToggleMenu);
    }

    public void ToggleMenu()
    {
        if (isExpanded)
        {
            StartCoroutine(AnimateMenu(false));
        }
        else
        {
            StartCoroutine(AnimateMenu(true));
        }
    }

    private IEnumerator AnimateMenu(bool expand)
    {
        isExpanded = expand;
        float timer = 0;

        foreach (Transform button in _childButtons)
        {
            button.gameObject.SetActive(true);
        }

        while (timer < _expandDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _expandDuration;
            float scaleValue = _expandCurve.Evaluate(t);

            foreach (Transform button in _childButtons)
            {
                button.localScale = expand ? Vector3.one * scaleValue : Vector3.one * (1 - scaleValue);
            }

            yield return null;
        }

        foreach (Transform button in _childButtons)
        {
            button.localScale = expand ? Vector3.one : Vector3.zero;

            if (!expand)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
