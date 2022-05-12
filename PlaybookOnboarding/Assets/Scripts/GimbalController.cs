using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransformMode { Translate, Rotate, Scale }
public class GimbalController : MonoBehaviour, ISelectable
{
    public TransformMode transformMode;
    [SerializeField] Transform gimbalTransform;

    private Vector3 mOffset;
    private float mSensitivity = 1f;
    private Vector3 mPreviousMousePos;
    private Vector3 mLocalPosition;

    private void Update()
    {
        if (InputController.instance.selectedObject)
            gimbalTransform.position = InputController.instance.selectedObject.transform.position;
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
        
        switch (transformMode)
        {
            case TransformMode.Translate:
                //gimbalTransform.position = currentMousePos + (InputController.instance.selectedObject.transform.position - Vector3.Project(transform.position, direction)) + mOffset + transform.parent.localPosition;
                InputController.instance.selectedObject.Translate((currentMousePos - mPreviousMousePos) * mSensitivity);
                gimbalTransform.Translate((currentMousePos - mPreviousMousePos) * mSensitivity);
                break;
            case TransformMode.Rotate:
                transform.parent.position = currentMousePos + (InputController.instance.selectedObject.transform.position - Vector3.Project(transform.position, direction)) + mOffset;
                InputController.instance.selectedObject.Rotate((currentMousePos - mPreviousMousePos) * 10 * mSensitivity);
                gimbalTransform.Rotate((currentMousePos - mPreviousMousePos) * 10 * mSensitivity);
                break;
            case TransformMode.Scale:
                var newScale = InputController.instance.selectedObject.localScale + (currentMousePos - mPreviousMousePos) * mSensitivity;
                if (newScale.x > 0 && newScale.y > 0 && newScale.z > 0)
                {
                    transform.parent.position = currentMousePos + (InputController.instance.selectedObject.transform.position - Vector3.Project(transform.position, -direction)) + mOffset - transform.localPosition;
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
