using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformable : MonoBehaviour, ISelectable
{
    public List<GimbalValue> gimbalValues = new List<GimbalValue>();

    private Vector3 mOffset;

    private void OnEnable()
    {
        if (InputController.instance.gimbalManager)
            InputController.instance.gimbalManager.SaveGimbalValues(gimbalValues, InputController.instance.gimbalManager.defaultValues);
    }

    public void Select()
    {
        InputController.instance.previousSelectable?.Deselect();

        InputController.instance.selectedObject = transform;
        ToggleSelectedObjectUI(true);
        mOffset = InputController.instance.GetMousePos() - transform.position;
    }

    public void Deselect()
    {
        ToggleSelectedObjectUI(false);
        InputController.instance.selectedObject = null;
    }

    public void Drag()
    {
        transform.position = InputController.instance.GetMousePos() - mOffset;
    }

    public void Drop()
    {
    }

    public void ToggleSelectedObjectUI(bool toggleOn)
    {
        var selectedObj = InputController.instance.selectedObject;
        if (selectedObj && selectedObj.GetComponent<Outline>())
            selectedObj.GetComponent<Outline>().enabled = toggleOn;
        InputController.instance.gimbal.SetActive(toggleOn);

        if (InputController.instance.gimbalManager)
        {
            if (toggleOn)
                InputController.instance.gimbalManager.LoadGimbalValues(InputController.instance.gimbalManager.gimbals, gimbalValues);
            else
                InputController.instance.gimbalManager.SaveGimbalValues(gimbalValues, InputController.instance.gimbalManager.gimbals);
        }
    }
}
