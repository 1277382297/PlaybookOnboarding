using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGimbal : GimbalController
{
    [SerializeField] private bool isX = false, isY = false, isZ = false;
    private Vector3 mPerpendicularDirection;

    public override void Select()
    {
        mOffset = transform.position - InputController.instance.selectedObject.transform.position;
        mPreviousMousePos = Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), mOffset.normalized);

        if (isX)
            mPerpendicularDirection = InputController.instance.selectedObject.up;
        else if (isY)
            mPerpendicularDirection = InputController.instance.selectedObject.right;
        else if (isZ)
            mPerpendicularDirection = InputController.instance.selectedObject.forward;
    }

    public override void Drag()
    {
        var direction = mOffset.normalized;
        var currentMousePos = Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), direction);
        var angle = Vector3.SignedAngle(mPreviousMousePos - InputController.instance.selectedObject.position, currentMousePos - InputController.instance.selectedObject.position, direction);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(currentMousePos - mPreviousMousePos);
        InputController.instance.selectedObject.Rotate(localAmountCursorMoved * mSensitivity);
        InputController.instance.gimbal.transform.Rotate(localAmountCursorMoved * mSensitivity);
        mPreviousMousePos = currentMousePos;
    }
}
