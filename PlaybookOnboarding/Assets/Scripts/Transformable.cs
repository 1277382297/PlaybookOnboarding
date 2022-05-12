using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformable : MonoBehaviour, ISelectable
{
    public void Select()
    {
        if (InputController.instance.previousSelectable != null && InputController.instance.previousSelectable != this)
            InputController.instance.previousSelectable.Deselect();

        InputController.instance.selectedObject = transform;
        var selectedObj = InputController.instance.selectedObject;
        InputController.instance.offset = InputController.instance.GetMousePos() - selectedObj.position;

        if (InputController.instance.hitObject != selectedObj)
        {
            ToggleSelectedObjectUI(false);
            selectedObj = InputController.instance.hitObject;
        }
        InputController.instance.offset = InputController.instance.GetMousePos() - selectedObj.transform.position;
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
            InputController.instance.selectedObject.transform.position = InputController.instance.GetMousePos() + InputController.instance.offset;
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
