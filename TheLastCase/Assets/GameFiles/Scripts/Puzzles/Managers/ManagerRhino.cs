using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ManagerRhino : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    [Header("Used Objects")]
    public CinemachineVirtualCamera VirtualCamera;
    public Button placeItemButton;
    public Button pickUpItemButton;
    public GameObject inventoryPanel;
    public GameObject rhinoHornSil;
    public GameObject ballObjPuzzle;
    public Animator animator;

    [HideInInspector] public Transform hitSlot;
    private CinemachineBrain gameCam;
    private bool puzzleCompleted = false;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
    }

    private void Update()
    {
        if (rhinoHornSil.transform.parent.transform.GetChild(1).GetComponent<InspectionChecker>().enabled && !puzzleCompleted)
        {
            HoveringOnSlot();
        }
    }

    public void HoveringOnSlot()
    {
        gameCam.OutputCamera.transform.position = VirtualCamera.transform.position;
        gameCam.OutputCamera.transform.rotation = VirtualCamera.transform.rotation;

        Ray ray = gameCam.OutputCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask("PuzzleSlot")) && !inventoryPanel.activeInHierarchy)
        {
            if (hit.transform.tag == "Silhouette")
            {

                hitSlot = hit.transform;
                ShowButtonInFrontOfSlot(hit.collider.gameObject, hit.point);
            }
            else
            {
                placeItemButton.gameObject.SetActive(false);
                pickUpItemButton.gameObject.SetActive(false);
            }
        }
        else
        {
            placeItemButton.gameObject.SetActive(false);
            pickUpItemButton.gameObject.SetActive(false);
        }
    }

    void ShowButtonInFrontOfSlot(GameObject hoveredSlot, Vector3 hitPoint)
    {
        GameObject activeButton = null;
        if (hitSlot.childCount > 0 || hitSlot.GetComponent<PuzzleData>().isOccupied)
        {
            activeButton = pickUpItemButton.gameObject;
        }
        else
        {
            activeButton = placeItemButton.gameObject;
        }
        activeButton.gameObject.SetActive(true);
        activeButton.transform.position = hitPoint;
        activeButton.transform.rotation = Quaternion.LookRotation(gameCam.OutputCamera.transform.forward);
    }

    public void CheckPuzzle()
    {
        if (hitSlot != null) 
        { 
            if (hitSlot.GetComponent<PuzzleData>().correctItem == hitSlot.transform.GetChild(0).GetComponent<InteractableObject>().itemData)
            { 
                PuzzleComplete(); 
                hitSlot.transform.GetChild(0).transform.rotation = rhinoHornSil.transform.rotation;
            } 
        }
    }

    public void PuzzleComplete()
    {
        puzzleCompleted = true;
        PuzzleRegistry.Instance.PuzzleFinished();
        GameObject.Find("Button_Interact").GetComponent<Button>().onClick.RemoveAllListeners();
        pickUpItemButton.onClick.RemoveAllListeners();
        placeItemButton.onClick.RemoveAllListeners();
        animator.SetTrigger("PuzzleComplete");
    }
}
