using UnityEngine;

public class ManagerPedestalPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    [Header("Used Objects")]
    public GameObject[] pedestals;
    public Animator animator;
    [SerializeField] GameObject note;

    private void Start()
    {
        note.SetActive(false);
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
    }

    public void CheckPuzzle()
    {
        int correctFigureHeadsPlaced = 0;

        foreach (GameObject pedestal in pedestals)
        {
            Transform collider = pedestal.transform.Find("Collider");
            PuzzleData puzzleSlotData = collider.GetComponent<PuzzleData>();

            InventoryItemData itemData = null;

            // Check to see if position is available for item
            if (puzzleSlotData.itemHolder.childCount > 0)
            {
                itemData = puzzleSlotData.itemHolder.GetChild(0).GetComponent<InteractableObject>().itemData;
                puzzleSlotData.isOccupied = true;

            }
            else
            {
                puzzleSlotData.isOccupied = false;
            }

            if (puzzleSlotData.correctItem == itemData && itemData != null)
            {
                correctFigureHeadsPlaced++;
            }
        }

        // Check all three items are in correct slots
        if (correctFigureHeadsPlaced == 3)
        {        
            PuzzleComplete();          
        }
    }

    public void PuzzleComplete()
    {
        foreach (GameObject pedestal in pedestals)
        {
            Transform collider = pedestal.transform.Find("Collider");
            PuzzleData puzzleSlotData = collider.GetComponent<PuzzleData>();

            collider.gameObject.SetActive(false);

            BoxCollider itemCollider = puzzleSlotData.itemHolder.GetChild(0).GetComponent<BoxCollider>();
            itemCollider.enabled = false;
        }

        note.SetActive(true);
        animator.SetTrigger("PuzzleComplete");

        Debug.Log("Puzzle Complete");
    }
}
