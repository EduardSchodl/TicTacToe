using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{

    private Camera mainCamera;

    private bool startedDragging = false;

    private Vector3 dragStartPos;
    
    private Vector3 lastMousePos;
    
    public bool HasStartedDragging()
    {
        return startedDragging;
    }

    public Vector3 GetDragStartPos()
    {
        return dragStartPos;
    }

    public Vector3 GetDragDiff()
    {
        return Input.mousePosition - dragStartPos;
    }

    public Vector3 GetLastDragDiff()
    {
        return Input.mousePosition - lastMousePos;
    }

    public Vector3 GetLastDragPosWorld()
    {
        return mainCamera.ScreenToWorldPoint(lastMousePos);
    }

    public Vector3 GetLastDragDiffWorld()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition) - GetLastDragPosWorld();
    }

    private List<IOnDrag> onDrags = new List<IOnDrag>();
    private List<IOnDragStart> onDragStarts = new List<IOnDragStart>();
    private List<IOnDragEnd> onDragEnds = new List<IOnDragEnd>();

    void Start()
    {
        mainCamera = Camera.main;
        onDrags.AddRange(GetComponents<IOnDrag>());
        onDragStarts.AddRange(GetComponents<IOnDragStart>());
        onDragEnds.AddRange(GetComponents<IOnDragEnd>());
    }

    public void AddOnDragStart(IOnDragStart onDragStart)
    {
        onDragStarts.Add(onDragStart);
    }

    public void AddOnDrag(IOnDrag onDrag)
    {
        onDrags.Add(onDrag);
    }

    public void AddOnDragEnd(IOnDragEnd onDragEnd)
    {
        onDragEnds.Add(onDragEnd);
    }


    private void OnDragStart()
    {
        foreach(IOnDragStart onDragStart in onDragStarts)
        {
            onDragStart.OnDragStart();
        }
    }

    private void OnDragEnd()
    {
        foreach (IOnDragEnd onDragEnd in onDragEnds)
        {
            onDragEnd.OnDragEnd();
        }
    }

    private void OnDrag()
    {
        foreach (IOnDrag onDrag in onDrags)
        {
            onDrag.OnDrag();
        }
    }

    void Update()
    {
        Vector3 currentMousePos = Input.mousePosition;
        bool lastPosIsCurr = currentMousePos.VecEqualNoZ(lastMousePos);
        
        // OnDrag
        if (startedDragging && Input.GetMouseButton(0))
        {
            OnDrag();
        }

        // OnDragStart
        if (!startedDragging && Input.GetMouseButton(0) && !lastPosIsCurr)
        {
            dragStartPos = currentMousePos;
            lastMousePos = dragStartPos;

            startedDragging = true;
            OnDragStart();
        }

        // OnDragEnd
        if(startedDragging && Input.GetMouseButtonUp(0))
        {
            startedDragging = false;
            OnDragEnd();
        }

        lastMousePos = currentMousePos;
    }
}
