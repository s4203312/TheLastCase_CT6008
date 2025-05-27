using UnityEngine;

public class GhostPosession : MonoBehaviour
{
    private float zPosition;
    private float orgYPos;
    private Vector3 offset;
    public GameObject selectedItem;

    void Update()
    {
        if (selectedItem != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedItem = null;
            }
            else
            {
                MoveItem();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectItem();
            }
        }
    }

    void SelectItem()
    {
        Camera cam = Camera.main;
        if (cam != null && cam.enabled)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Posessable"))
                {
                    selectedItem = hit.transform.gameObject;

                    orgYPos = selectedItem.transform.position.y;

                    //Locking the z pos
                    zPosition = cam.WorldToScreenPoint(selectedItem.transform.position).z;
                    offset = selectedItem.transform.position - GetMouseWorldPosition(cam);
                }
            }
        }
    }

    //Moving item along the z axis when pick up
    void MoveItem()
    {
        selectedItem.transform.position = GetMouseWorldPosition(Camera.main) + offset;
        Vector3 pos = selectedItem.transform.position;
        pos.y = orgYPos;
        selectedItem.transform.position = pos;

        PuzzleRegistry.Instance.CheckPuzzleByID(selectedItem.GetComponent<InteractableObject>().itemData.puzzleID);
    }

    Vector3 GetMouseWorldPosition(Camera cam)
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zPosition;
        return cam.ScreenToWorldPoint(mousePoint);
    }

}
