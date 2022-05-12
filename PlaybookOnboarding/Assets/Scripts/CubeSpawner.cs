using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour, ISelectable
{
    [SerializeField] private Color grabbedObjectOutlineColor = Color.yellow;

    public void Select()
    {
        SpawnCube(InputController.instance.GetMousePos());
    }
    
    public void Deselect()
    {
    }

    public void Drag()
    {
        
    }

    public void Drop()
    {

    }

    private void SpawnCube(Vector3 spawnPosition)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spawnPosition.z = 0;
        cube.transform.position = spawnPosition;

        // add outline script
        var outline = cube.AddComponent<Outline>();
        outline.enabled = false;
        outline.OutlineColor = grabbedObjectOutlineColor;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 10;

        cube.AddComponent<Transformable>();

        if (InputController.instance.previousSelectable != null)
            InputController.instance.previousSelectable.Deselect();
        InputController.instance.selectedObject = cube.transform;
        InputController.instance.hitObject = cube.transform;
        InputController.instance.isGrabbing = true;

    }
}
