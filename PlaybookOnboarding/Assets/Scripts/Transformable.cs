using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformable : MonoBehaviour, ISelectable
{
    private Vector3 mOffset;

    public void Select()
    {
        InputController.instance.previousSelectable.Deselect();

        InputController.instance.selectedObject = transform;
        mOffset = InputController.instance.GetMousePos() - transform.position;

        if (InputController.instance.hitObject == transform)
        {
            ToggleSelectedObjectUI(false);
            InputController.instance.selectedObject = InputController.instance.hitObject;
        }
    }

    public void Deselect()
    {
        ToggleSelectedObjectUI(false);
        InputController.instance.selectedObject = null;
    }

    public void Drag()
    {
        ToggleSelectedObjectUI(true);
        if (InputController.instance.isGrabbing)
        {
            transform.position = InputController.instance.GetMousePos() - mOffset;
        }
    }

    public void Drop()
    {
    }

    public void ToggleSelectedObjectUI(bool toggleOn)
    {
        var selectedObj = InputController.instance.selectedObject;
        if (selectedObj && selectedObj.GetComponent<Outline>())
            selectedObj.GetComponent<Outline>().enabled = toggleOn;

        if (selectedObj)
            InputController.instance.gimbal.transform.position = selectedObj.position;
        InputController.instance.gimbal.SetActive(toggleOn);
    }
}
