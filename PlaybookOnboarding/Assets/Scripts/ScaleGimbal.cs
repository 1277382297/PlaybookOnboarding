using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGimbal : GimbalController
{
    [SerializeField] protected Transform rotationGimbals;

    public override void Select()
    {
        mOffset = transform.parent.position - InputController.instance.selectedObject.transform.position;
        mPreviousMousePos = Vector3.Project(InputController.instance.GetMousePos(), mOffset.normalized);
    }

    public override void Drag()
    {
        var direction = mOffset.normalized;
        var currentMousePos = Vector3.Project(InputController.instance.GetMousePos(), direction);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(currentMousePos - mPreviousMousePos);
        var newScale = InputController.instance.selectedObject.localScale + localAmountCursorMoved * mSensitivity;
        if (newScale.x > 0 && newScale.y > 0 && newScale.z > 0)
        {
            transform.parent.Translate(localAmountCursorMoved);
            rotationGimbals.localScale += localAmountCursorMoved;
            InputController.instance.selectedObject.localScale = newScale;
        }
        mPreviousMousePos = currentMousePos;
    }
}
