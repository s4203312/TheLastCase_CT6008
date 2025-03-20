using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostTetherRenderer : MonoBehaviour
{
    //Start and end points for the line
    private GameObject player;
    private GameObject ghost;

    private LineRenderer lineRenderer;
    public List<Vector3> linePoints = new List<Vector3>();

    //Custom shader graph material
    public Material tetherMat;

    void Start()
    {
        //Finding the player and ghost from parent object
        player = transform.GetChild(0).gameObject;
        ghost = transform.GetChild(1).gameObject;

        //Adding a line renderer to the ghost object if it doesnt have one
        if (ghost.TryGetComponent<LineRenderer>(out LineRenderer objectLineRenderer))
        {
            lineRenderer = objectLineRenderer;
        }
        else
        {
            lineRenderer = ghost.AddComponent<LineRenderer>();
        }
        
        //Setting up line renderer
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = tetherMat;
        lineRenderer.useWorldSpace = true;

        //Defining points at start
        linePoints.Add(player.transform.position);
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    void Update()
    {
        Vector3 currentEndPoint = ghost.transform.position;
        int amountOfPoints = linePoints.Count;

        if (amountOfPoints == 0 || Vector3.Distance(linePoints[amountOfPoints - 1], currentEndPoint) >= 0.2)    //Ensuring the new point is 0.2 away from the last one    
        {
            linePoints.Add(currentEndPoint);
            UpdateTether(amountOfPoints + 1);       //One extra point for the end point of the ghost
        }
    }

    private void UpdateTether(int amountOfPoints)
    {
        lineRenderer.positionCount = amountOfPoints;

        int i = 0;
        foreach (var point in linePoints)       //Looping all points and reseting the renderer
        {
            lineRenderer.SetPosition(i, point);
            i++;
        }

        lineRenderer.SetPosition(amountOfPoints, ghost.transform.position);     //Last point for the ghost position
        
    }
}
