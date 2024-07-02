using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DragScript))]
public class CameraDrag : MonoBehaviour, IOnDrag
{

    private DragScript dragScript;

    void Start()
    {
        dragScript = GetComponent<DragScript>();
    }

    public void OnDrag()
    {
        Vector3 diff = dragScript.GetLastDragDiffWorld() * -1;
        transform.position = transform.position + diff;
    }

}
