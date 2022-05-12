using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void Select();
    public void Deselect();
    public void Drag();
    public void Drop();
}
