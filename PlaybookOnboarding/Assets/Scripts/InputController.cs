using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject gimbalPrefab;

    public Transform selectedObject { get { return mSelectedObject; } set { mSelectedObject = value; } }
    public bool isGrabbing { get { return bIsGrabbing; } set { bIsGrabbing = value; } }
    public GameObject gimbal { get { return mGimbal; } }
    public Transform hitObject { get { return mHitObject; } set { mHitObject = value; } }
    public ISelectable previousSelectable { get { return mPreviousSelectable; } }
    public Transform[] gimbalsList { get { return mGimbalsList; } }

    private Transform mSelectedObject = null;
    private bool bIsGrabbing = false;
    private GameObject mGimbal = null;
    private Transform mHitObject;
    private ISelectable mPreviousSelectable = null;
    private Transform[] mGimbalsList;

    public static InputController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mGimbal = Instantiate(gimbalPrefab);
        mGimbal.SetActive(false);
        var gimbalManager = mGimbal.GetComponent<GimbalManager>();
        if (gimbalManager)
            mGimbalsList = gimbalManager.gimbals;
    }

    private void Update()
    {
        // selected object
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (mSelectedObject)
                mPreviousSelectable = mSelectedObject.GetComponent<ISelectable>();

            if (hit.collider)
            {
                var selectable = hit.transform.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    mHitObject = hit.collider.transform;
                    selectable.Select();
                }
            }
            else
            {
                if (mPreviousSelectable != null)
                    mPreviousSelectable.Deselect();
            }
            
        }

        // drag & dropping object
        if (mSelectedObject)
        {
            var selectable = mHitObject.GetComponent<ISelectable>();
            if (selectable != null)
            {
                if (Input.GetMouseButton(0))
                {
                    bIsGrabbing = true;
                    selectable.Drag();
                }
                else
                {
                    bIsGrabbing = false;
                    selectable.Drop();
                }
            }
        }
    }

    public Vector3 GetMousePos()
    {
        if (mSelectedObject)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(mSelectedObject.position).z);
            return (Camera.main.ScreenToWorldPoint(position));
        }
        return Vector3.zero;
    }

    private RaycastHit CastRay()
    {
        // referenced from https://www.youtube.com/watch?v=uNCCS6DjebA&t=469s

        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }     
}
