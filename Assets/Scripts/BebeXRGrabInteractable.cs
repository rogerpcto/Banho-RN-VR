using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BebeXRGrabInteractable : XRGrabInteractable
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.parent.name == "Left Controller")
            attachTransform.localRotation = Quaternion.Euler(0, 180, 90);
        else
            attachTransform.localRotation = Quaternion.Euler(0, 0, 90);

        base.OnSelectEntering(args);
    }
}
