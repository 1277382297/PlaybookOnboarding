using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GimbalValue
{
    public Vector3 gimbalPosition;
    public Quaternion gimbalRotation;
    public Vector3 gimbalScale;
}

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

    [SerializeField] private Transform[] mGimbals = new Transform[5];
    private GimbalValue[] mDefaultValues = new GimbalValue[5];

    private void Awake()
    {
        SaveGimbalValues(mDefaultValues, mGimbals);
    }

    public void LoadGimbalValues(Transform[] loadTo, GimbalValue[] loadFrom)
    {
        for (int i = 0; i < loadTo.Length; i++)
        {
            loadTo[i].localPosition = loadFrom[i].gimbalPosition;
            loadTo[i].localRotation = loadFrom[i].gimbalRotation;
            loadTo[i].localScale = loadFrom[i].gimbalScale;
        }
    }

    public void SaveGimbalValues(GimbalValue[] loadTo, Transform[] loadFrom)
    {
        for (int i = 0; i < loadTo.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = loadFrom[i].localPosition;
            values.gimbalRotation = loadFrom[i].localRotation;
            values.gimbalScale = loadFrom[i].localScale;
            loadTo[i] = values;
        }
    }

    public void SaveGimbalValues(GimbalValue[] loadTo, GimbalValue[] loadFrom)
    {
        for (int i = 0; i < loadTo.Length; i++)
        {
            var values = new GimbalValue();
            values.gimbalPosition = loadFrom[i].gimbalPosition;
            values.gimbalRotation = loadFrom[i].gimbalRotation;
            values.gimbalScale = loadFrom[i].gimbalScale;
            loadTo[i] = values;
        }
    }
}
