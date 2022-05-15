using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGimbal : GimbalController
{
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
        InputController.instance.selectedObject.Translate(localAmountCursorMoved * mSensitivity);
        InputController.instance.gimbal.transform.Translate(localAmountCursorMoved * mSensitivity);
        mPreviousMousePos = currentMousePos;
    }
}
