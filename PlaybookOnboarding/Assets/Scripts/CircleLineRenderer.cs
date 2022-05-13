using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLineRenderer : MonoBehaviour
{
    private LineRenderer mCircleRenderer;
    private MeshCollider mMeshCollider;

    private void Awake()
    {
        mCircleRenderer = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mMeshCollider = gameObject.AddComponent<MeshCollider>();
        RefreshCircle();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshCircle();
    }

    void RefreshCircle()
    {
        var v3 = InputController.instance.selectedObject.localScale;
        DrawCircle(100, Mathf.Max(Mathf.Max(v3.x, v3.y), v3.z));
        Mesh mesh = new Mesh();
        mCircleRenderer.BakeMesh(mesh, false);
        mMeshCollider.sharedMesh = mesh;
        //mMeshCollider.convex = true;
    }

    void DrawCircle(int steps, float radius)
    {
        // referenced from https://www.youtube.com/watch?v=DdAfwHYNFOE
        mCircleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            mCircleRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
