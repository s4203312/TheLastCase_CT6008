using Cinemachine;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractActions : MonoBehaviour
{
    private GameObject interactionObject;
    private GameObject ghost;
    public Button interactButton;
    public Button inventoryButton;

    public GameObject Managers;

    public ShelfHover shelfHover;

    public bool isUsingButton;

    private float cooldownTime = 0.5f;
    private float lastClickTime = 0f;

    [HideInInspector] public Vector3 oldCameraPos;
    [HideInInspector] public Quaternion oldCameraRot;

    private void Start()
    {
        ghost = transform.GetChild(1).gameObject;
    }

    //Player Actions

    public void OpenDoor()
    {
        if (OnCallOnce()) return;

        interactionObject = GetComponent<PlayerMovement>().interactedObject;
        GameObject pivotPoint = interactionObject.transform.GetChild(0).gameObject;

        interactionObject.GetComponent<BoxCollider>().enabled = false;
        interactButton.gameObject.SetActive(false);

        StartCoroutine(OpeningDoor(pivotPoint));
    }

    public IEnumerator OpeningDoor(GameObject pivotPoint)
    {
        float targetAngle = Random.Range(75,105);
        float rotatedAngle = 0f;
        float rotationSpeed = 45f;

        while (rotatedAngle < targetAngle)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            interactionObject.transform.RotateAround(pivotPoint.transform.position, Vector3.down, rotationStep);
            rotatedAngle += rotationStep;
            yield return null;
        }
    }

    public void PickUpItem()
    {
        if (!isUsingButton)
        {
            interactionObject = GetComponent<PlayerMovement>().interactedObject;
        }
        else
        {
            if (GameObject.Find("StatuesManager").GetComponent<ManagerStatuesPuzzle>().hitSlot != null)
            {
                if (GameObject.Find("StatuesManager").GetComponent<ManagerStatuesPuzzle>().hitSlot.childCount > 0)
                {
                    interactionObject = GameObject.Find("StatuesManager").GetComponent<ManagerStatuesPuzzle>().hitSlot.GetChild(0).gameObject;
                    isUsingButton = false;
                }
            }
            else if (GameObject.Find("PuzzleManager").GetComponent<ManagerPosessionBallPuzzle>().hitSlot != null)
            {
                if (GameObject.Find("PuzzleManager").GetComponent<ManagerPosessionBallPuzzle>().hitSlot.childCount > 0)
                {
                    interactionObject = GameObject.Find("PuzzleManager").GetComponent<ManagerPosessionBallPuzzle>().hitSlot.GetChild(0).gameObject;
                    isUsingButton = false;
                }
            }
            else if (GameObject.Find("PuzzleManager").GetComponent<ManagerRhino>().hitSlot != null)
            {
                if (GameObject.Find("PuzzleManager").GetComponent<ManagerRhino>().hitSlot.childCount > 0)
                {
                    interactionObject = GameObject.Find("PuzzleManager").GetComponent<ManagerRhino>().hitSlot.GetChild(0).gameObject;
                    isUsingButton = false;
                }
            }        
            else if (shelfHover.hitSlot.childCount > 0)
            {
                interactionObject = shelfHover.hitSlot.GetChild(0).gameObject;
                isUsingButton = false;
            }
        }
        InventoryManager.Instance.AddItemToInventory(interactionObject.GetComponent<InteractableObject>().itemData, interactionObject);
        Transform inventoryManager = Managers.transform.Find("InventoryManager");
        inventoryManager.gameObject.GetComponent<UIInventoryLoad>().InventoryClose();
      
        interactionObject.transform.SetParent(null);

        if (interactionObject.GetComponent<InteractableObject>().itemData != null)
        {
            PuzzleRegistry.Instance.CheckPuzzleByID(interactionObject.GetComponent<InteractableObject>().itemData.puzzleID);
            PuzzleRegistry.Instance.CheckPuzzleByID(interactionObject.GetComponent<InteractableObject>().itemData.puzzleID);
        }

        if (interactionObject.transform.Find("SparkleEffect(Clone)"))
        {
            Destroy(interactionObject.transform.Find("SparkleEffect(Clone)").gameObject);
        }

        interactionObject.GetComponent<BoxCollider>().enabled = false;
        interactionObject.SetActive(false);
        interactButton.gameObject.SetActive(false);
    }

    public void PlaceItem(string itemPosition)
    {
        if (OnCallOnce()) return;

        interactionObject = GetComponent<PlayerMovement>().interactedObject;
        Transform inventoryManager = Managers.transform.Find("InventoryManager");

        // logic for deciding if the object has multiple slots for placing items
        Transform itemTransform = null;
        Transform[] slots = interactionObject.transform.parent.GetComponentsInChildren<Transform>(true);
        slots = slots.Where(t => t.name == "PuzzleSlot").ToArray();
             
        if (slots.Length > 1) 
        {
            if (shelfHover.enabled == true && shelfHover.hitSlot != null)
            {
                itemTransform = shelfHover.hitSlot;             
            }
        }
        else
        {
            itemTransform = interactionObject.transform.parent.Find("PuzzleSlot").transform;
        }

        int index = int.Parse(itemPosition) - 1;

        if (index >= 0 && index < InventoryManager.Instance.Inventory.Count)
        {
            InventoryItemData itemData = InventoryManager.Instance.Inventory[index];
            GameObject itemObject = InventoryManager.Instance.InventoryGameObjects[index];

            if (itemObject != null)
            {
                if (slots.Length > 1)
                {
                    itemObject.GetComponent<BoxCollider>().enabled = false;
                    itemObject.transform.rotation = itemTransform.rotation;
                }
                itemObject.transform.position = itemTransform.position;
                
                itemObject.transform.SetParent(itemTransform);
                itemObject.SetActive(true);

                PuzzleRegistry.Instance.CheckPuzzleByID(itemData.puzzleID);

                InventoryManager.Instance.RemoveItemFromInventory(itemData, itemObject);
                inventoryManager.gameObject.GetComponent<UIInventoryLoad>().InventoryClose();
                interactButton.gameObject.SetActive(false);
            }
        }
    }

    public void FocusOnPuzzle()
    {
        inventoryButton.gameObject.SetActive(false);
        interactionObject = GetComponent<PlayerMovement>().interactedObject;

        if (interactionObject.TryGetComponent(out InspectionChecker inspectionCheck)) 
        { 
            inspectionCheck.enabled = true;
        }

        GameUI gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        Button exitViewButton = gameUI.transform.GetChild(2).GetComponent<Button>();
        Transform newCamera = interactionObject.transform.parent.GetChild(0);
        //Transform[] newCameras = null;
        //newCameras.Append(newCamera);
        CinemachineVirtualCamera virtCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        CameraMove cameraMove = virtCam.GetComponent<CameraMove>();

        oldCameraPos = virtCam.transform.position;
        oldCameraRot = virtCam.transform.rotation;

        if (cameraMove != null)
        {
            GetComponent<PlayerMovement>().enabled = false;

            cameraMove.MoveCameraToRoom(newCamera.position, newCamera.rotation);

            StartCoroutine(DelayGameObject(2, exitViewButton.gameObject, true));          
            exitViewButton.onClick.AddListener(() => gameUI.ExitView(1));
        }
        gameObject.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        interactButton.GetComponent<Image>().enabled = false;
        interactButton.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void AccessInventory()
    {
        Transform inventoryManager = Managers.transform.Find("InventoryManager");

        inventoryManager.gameObject.GetComponent<UIInventoryLoad>().LoadInventory(false);
    }

    public void FinalDoorCheck()
    {
        if (PuzzleRegistry.Instance.puzzleCounter == 0)
        {
            GameObject finalDoor = GameObject.Find("ExitDoor");
            finalDoor.transform.Find("DoorCheckCollider").GetComponent<BoxCollider>().enabled = false;
            finalDoor.transform.GetChild(3).transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void CompleteGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Ghost Actions

    public void KeyholeSquish()
    {
        interactionObject = GetComponent<PlayerMovement>().interactedObject;

        ghost.GetComponent<NavMeshAgent>().Warp(ghost.transform.position + (-interactionObject.transform.right * 2));
    }



    // Extras
    private IEnumerator DelayGameObject(int time, GameObject gameObject, bool active)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(active);
    }

    public void UsingButton()
    {
        isUsingButton = true;
    }

    private bool OnCallOnce()
    {
        // breaks loop if function is called more than once in too many frames
        if (Time.time - lastClickTime < cooldownTime)
        {
            return true;
        }

        lastClickTime = Time.time;
        return false;
    }
}