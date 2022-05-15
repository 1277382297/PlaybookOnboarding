using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransformMode { Translate, Rotate, Scale }
public class GimbalController : MonoBehaviour, ISelectable
{
    public TransformMode transformMode;

    [SerializeField] private float mSensitivity = 1f;
    private Vector3 mOffset;
    private Vector3 mPreviousMousePos;
    private Vector3 mLocalPosition;

    public bool isX = false, isY= false, isZ = false;

    private Vector3 mPerpendicularDirection;

    private void Update()
    {
        if (InputController.instance.selectedObject && InputController.instance.gimbal.transform)
            InputController.instance.gimbal.transform.position = InputController.instance.selectedObject.transform.position;
    }

    public void Select()
    {
        switch (transformMode)
        {
            case TransformMode.Rotate:
                mOffset = transform.position - InputController.instance.selectedObject.transform.position;
                mPreviousMousePos = Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), mOffset.normalized);

                if (isX)
                    mPerpendicularDirection = InputController.instance.selectedObject.up;
                else if (isY)
                    mPerpendicularDirection = InputController.instance.selectedObject.right;
                else if (isZ)
                    mPerpendicularDirection = InputController.instance.selectedObject.forward;

                break;
            default:
                mOffset = transform.parent.position - InputController.instance.selectedObject.transform.position;
                mPreviousMousePos = Vector3.Project(InputController.instance.GetMousePos(), mOffset.normalized);
                break;
        }

        mLocalPosition = transform.parent.localPosition;
        
    }

    public void Deselect()
    {
    }

    public void Drag()
    {
        var direction = mOffset.normalized;
        var currentMousePos = Vector3.Project(InputController.instance.GetMousePos(), direction);
        var localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(currentMousePos - mPreviousMousePos);
        switch (transformMode)
        {
            case TransformMode.Translate:
                InputController.instance.selectedObject.Translate(localAmountCursorMoved * mSensitivity);
                InputController.instance.gimbal.transform.Translate(localAmountCursorMoved * mSensitivity);
                break;
            case TransformMode.Rotate:
                currentMousePos = Vector3.ProjectOnPlane(InputController.instance.GetMousePos(), direction);
                var angle = Vector3.SignedAngle(mPreviousMousePos - InputController.instance.selectedObject.position, currentMousePos - InputController.instance.selectedObject.position, direction);
                localAmountCursorMoved = InputController.instance.selectedObject.InverseTransformDirection(angle * mPerpendicularDirection);
                InputController.instance.selectedObject.Rotate(localAmountCursorMoved * mSensitivity);
                InputController.instance.gimbal.transform.Rotate(localAmountCursorMoved * mSensitivity);
                break;
            case TransformMode.Scale:
                var newScale = InputController.instance.selectedObject.localScale + localAmountCursorMoved * mSensitivity;
                if (newScale.x > 0 && newScale.y > 0 && newScale.z > 0)
                {
                    transform.parent.Translate(localAmountCursorMoved);
                    InputController.instance.selectedObject.localScale = newScale;
                }
                break;
        }

        mPreviousMousePos = currentMousePos;
    }

    public void Drop()
    {
        switch (transformMode)
        {
            case TransformMode.Rotate:
                StartCoroutine(PrettyLerpyUwu(mLocalPosition, 15));
                //transform.parent.localPosition = mLocalPosition;
                break;
        }
    }

    private IEnumerator PrettyLerpyUwu(Vector3 endPos, float step)
    {
        var timeElapsed = 0f;
        var startPos = transform.parent.localPosition;
        while (Vector3.Distance(transform.parent.localPosition, endPos) > 0.01f)
        {
            transform.parent.localPosition = Vector3.Lerp(startPos, endPos, timeElapsed);
            timeElapsed += Time.deltaTime * step;
            yield return null;
        }

    }
}
