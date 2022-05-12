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

    private void Update()
    {
        if (InputController.instance.selectedObject && InputController.instance.gimbal.transform)
            InputController.instance.gimbal.transform.position = InputController.instance.selectedObject.transform.position;
    }

    public void Select()
    {
        mOffset = transform.parent.position - InputController.instance.selectedObject.transform.position;
        mLocalPosition = transform.parent.localPosition;
        mPreviousMousePos = Vector3.Project(InputController.instance.GetMousePos(), mOffset.normalized);
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
                InputController.instance.selectedObject.Rotate(localAmountCursorMoved * mSensitivity);
                InputController.instance.gimbal.transform.Rotate(localAmountCursorMoved * mSensitivity);
                transform.parent.Translate(localAmountCursorMoved);
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
                transform.parent.localPosition = mLocalPosition;
                break;
        }
    }
}
