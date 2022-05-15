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

        if (isX)
            mPerpendicularDirection = InputController.instance.selectedObject.up;
        else if (isY)
            mPerpendicularDirection = InputController.instance.selectedObject.right;
        else if (isZ)
            mPerpendicularDirection = InputController.instance.selectedObject.forward;

        mPreviousMousePos = GetMousePosWithDepth();
    }

    public override void Drag()
    {
        var currentMousePos = GetMousePosWithDepth();
        //var currentMousePos = Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), mPerpendicularDirection) + Vector3.Project(transform.position, mPerpendicularDirection);
        var angle = Vector3.SignedAngle(mPreviousMousePos - InputController.instance.selectedObject.position,
                                        currentMousePos - InputController.instance.selectedObject.position,
                                        mPerpendicularDirection);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(angle * mPerpendicularDirection);
        InputController.instance.selectedObject.Rotate(localAmountCursorMoved * mSensitivity);
        InputController.instance.gimbal.transform.Rotate(localAmountCursorMoved * mSensitivity);
        mPreviousMousePos = currentMousePos;
    }

    private Vector3 GetMousePosWithDepth()
    {
        // referenced from https://www.habrador.com/tutorials/math/4-plane-ray-intersection/
        Vector3 p_0 = InputController.instance.selectedObject.position;
        Vector3 n = mPerpendicularDirection;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
        Vector3 l_0 = ray.origin;
        Vector3 l = ray.direction;

        float denominator = Vector3.Dot(l, n);

        Vector3 p;
        if (!Mathf.Approximately(denominator, 0))
        {
            float t = Vector3.Dot(p_0 - l_0, n) / denominator;
            p = l_0 + l * t;
            return p;
        }
        return Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), mPerpendicularDirection) + Vector3.Project(transform.position, mPerpendicularDirection);
    }
}
