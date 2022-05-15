using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimbalController : MonoBehaviour, ISelectable
{
    [SerializeField] protected float mSensitivity = 1f;
    protected Vector3 mOffset;
    protected Vector3 mPreviousMousePos;

    private void Update()
    {
        if (InputController.instance.selectedObject && InputController.instance.gimbal.transform)
            InputController.instance.gimbal.transform.position = InputController.instance.selectedObject.transform.position;
    }

    public virtual void Select()
    {
    }

    public virtual void Deselect()
    {
    }

    public virtual void Drag()
    {
    }

    public virtual void Drop()
    {
    }
}
