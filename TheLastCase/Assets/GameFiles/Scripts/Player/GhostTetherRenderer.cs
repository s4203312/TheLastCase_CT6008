using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTetherRenderer : MonoBehaviour
{
    private GameObject player;
    private GameObject ghost;
    private LineRenderer lineRenderer;
    public Material tetherMat;

    void Start()
    {
        player = transform.GetChild(0).gameObject;
        ghost = transform.GetChild(1).gameObject;

        lineRenderer = ghost.AddComponent<LineRenderer>();

        //Setting up line renderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;

        //Ghostly colour. Needs changing with our material
        lineRenderer.material = tetherMat;
        lineRenderer.startColor = new Color(0.5f, 1f, 1f, 0.5f); // Cyan with transparency
        lineRenderer.endColor = new Color(0.5f, 1f, 1f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(ghost.transform.position, player.transform.position);
        float width = Mathf.Clamp(distance * 0.02f, 0.05f, 0.3f); // Adjust min/max width
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width * 0.5f;

        if (player != null && ghost != null)
        {
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, ghost.transform.position);
        }
    }
}
