using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimbalManager : MonoBehaviour
{
    public Transform[] gimbals
    {
        get { return mGimbals; }
    }

    public GimbalValue[] defaultValues
    {
        get { return mDefaultValues; }
    }

    [SerializeField] private Transform[] mGimbals = new Transform[4];
    private GimbalValue[] mDefaultValues = new GimbalValue[4];

    private void Awake()
    {
        for (int i = 0; i < mDefaultValues.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = mGimbals[i].localPosition;
            values.gimbalRotation = mGimbals[i].localRotation;
            values.gimbalScale = mGimbals[i].localScale;
            mDefaultValues[i] = values;
        }
    }
}
