using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimbalManager : MonoBehaviour
{
    public Transform[] gimbals
    {
        get { return mGimbals; }
    }

    [SerializeField] private Transform[] mGimbals = new Transform[4];
}
