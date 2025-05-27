using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Button exitView;
    private bool paused;
    public GameObject pausePanel;
    public GameObject interactButton;
    public GameObject inventoryButton;

    private Vector3 oldCameraPos;
    private Quaternion oldCameraRot;
    private CinemachineVirtualCamera gameCam;
    private GameObject playerCharacters;

    [Header("Inspection UI")]
    public GameObject inspectionUIPanel;
    public TextMeshProUGUI itemNameText;
    public Image itemSprite;
    public TextMeshProUGUI itemDescriptionText;

    private void Start()
    {
        pausePanel.SetActive(false);
        inspectionUIPanel.SetActive(false);
        exitView.gameObject.SetActive(false);

        gameCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        playerCharacters = GameObject.Find("PlayerCharacters");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (paused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    //Modular function for returning view back to the player
    public void ExitView(int index)
    {
        CameraMove cameraMove = gameCam.GetComponent<CameraMove>();

        UIInventoryLoad inventoryLoad = GameObject.Find("InventoryManager").GetComponent<UIInventoryLoad>();

        if (index == 1)
        {
            oldCameraPos = playerCharacters.GetComponent<InteractActions>().oldCameraPos;
            oldCameraRot = playerCharacters.GetComponent<InteractActions>().oldCameraRot;
        }
        else if (index == 2)
        {
            oldCameraPos = inventoryLoad.oldCameraPos;
            oldCameraRot = inventoryLoad.oldCameraRot;
           
            if (inventoryLoad.currentlyInspectingObject.activeInHierarchy == true)
            {
                inventoryLoad.currentlyInspectingObject.SetActive(false);
                inventoryLoad.currentlyInspectingObject = null;
                Hide();
            }
        }

        if (cameraMove != null)
        {
            cameraMove.MoveCameraToRoom(oldCameraPos, oldCameraRot);

            playerCharacters.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);

            exitView.gameObject.SetActive(false);

            StartCoroutine(DelayCameraFollow(1.75f, cameraMove));
        }

        if (playerCharacters.GetComponent<PlayerMovement>().interactedObject.TryGetComponent(out InspectionChecker inspectionCheck))
        {
            inspectionCheck.enabled = false;
        }

        inventoryButton.gameObject.SetActive(true);
        playerCharacters.transform.GetChild(0).transform.GetChild(4).transform.gameObject.SetActive(false);
        exitView.onClick.RemoveAllListeners();
    }

    public void ShowItem(InventoryItemData item)
    {
        itemNameText.text = $"Name : {item.itemName}";
        itemSprite.sprite = item.itemPicture;
        itemDescriptionText.text = $"Description : {item.itemDescription}";

        inspectionUIPanel.SetActive(true);
    }

    public void Hide() { inspectionUIPanel.SetActive(false); }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    private IEnumerator DelayCameraFollow(float time, CameraMove cameraMove)
    {
        yield return new WaitForSeconds(time);
        
        playerCharacters.GetComponent<PlayerMovement>().enabled = true;
        interactButton.GetComponent<Image>().enabled = true;
        interactButton.transform.GetChild(0).gameObject.SetActive(true);
        cameraMove.FollowPlayer();
    }
}
