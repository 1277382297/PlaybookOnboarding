using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GimbalValue
{
    public Vector3 gimbalPosition;
    public Quaternion gimbalRotation;
    public Vector3 gimbalScale;
}

public class Transformable : MonoBehaviour, ISelectable
{
    public GimbalValue[] gimbalValues = new GimbalValue[4];

    private Vector3 mOffset;

    private void OnEnable()
    {
        for (int i = 0; i < gimbalValues.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = InputController.instance.defaultGimbalValues[i].gimbalPosition;
            values.gimbalRotation = InputController.instance.defaultGimbalValues[i].gimbalRotation;
            values.gimbalScale = InputController.instance.defaultGimbalValues[i].gimbalScale;
            gimbalValues[i] = values;
        }
    }

    public void Select()
    {
        InputController.instance.previousSelectable?.Deselect();

        InputController.instance.selectedObject = transform;
        ToggleSelectedObjectUI(true);
        mOffset = InputController.instance.GetMousePos() - transform.position;

        /*if (InputController.instance.hitObject == transform)
        {
            ToggleSelectedObjectUI(false);
            InputController.instance.selectedObject = InputController.instance.hitObject;
        }*/
    }

    public void Deselect()
    {
        ToggleSelectedObjectUI(false);
        InputController.instance.selectedObject = null;
    }

    public void Drag()
    {
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
        InputController.instance.gimbal.SetActive(toggleOn);

        if (toggleOn)
            LoadGimbalValues();
        else
            SaveGimbalValues();
    }

    private void LoadGimbalValues()
    {
        for (int i = 0; i < gimbalValues.Length; i++)
        {
            InputController.instance.gimbalsList[i].localPosition = gimbalValues[i].gimbalPosition;
            InputController.instance.gimbalsList[i].localRotation = gimbalValues[i].gimbalRotation;
            InputController.instance.gimbalsList[i].localScale = gimbalValues[i].gimbalScale;
        }
    }

    private void SaveGimbalValues()
    {
        for (int i = 0; i < gimbalValues.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = InputController.instance.gimbalsList[i].localPosition;
            values.gimbalRotation = InputController.instance.gimbalsList[i].localRotation;
            values.gimbalScale = InputController.instance.gimbalsList[i].localScale;
            gimbalValues[i] = values;
        }
    }
}
