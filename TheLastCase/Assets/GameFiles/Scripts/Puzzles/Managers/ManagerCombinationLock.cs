using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerCombinationLock : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    [Header("Combination Info")]
    public int[] correctCombination = { 0, 0, 0, 0 };  // Set the correct code
    public TMP_Text[] numberDisplays;  // UI text to show numbers

    [Header("Buttons")]
    public Button[] increaseButtons;   // Buttons for increasing numbers
    public Button[] decreaseButtons;   // Buttons for decreasing numbers

    private int[] currentNumbers;  // Stores selected numbers
    private GameUI gameUI;

    [Header("Used Objects")]
    public GameObject lockObj;
    public GameObject lockCam;
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject lockedDoor;
    public GameObject lockCollider;
    public Button playerButton;
    public GameObject canvas;

    [Header("SFX")]
    public AudioClip incorrectSFX;
    public AudioClip correctSFX;

    void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();

        lockedDoor.GetComponent<BoxCollider>().enabled = false;

        int length = numberDisplays.Length;
        currentNumbers = new int[length];

        // Initialize display numbers
        for (int i = 0; i < length; i++)
        {
            int index = i;

            increaseButtons[i].onClick.AddListener(() => ChangeNumber(index, 1));
            decreaseButtons[i].onClick.AddListener(() => ChangeNumber(index, -1));

            currentNumbers[i] = 0;
            UpdateDisplay(index);
        }
    }

    public void ChangeNumber(int index, int change)
    {
        currentNumbers[index] = (currentNumbers[index] + change + 10) % 10; // Cycle between 0-9
        UpdateDisplay(index);
    }

    void UpdateDisplay(int index)
    {
        numberDisplays[index].text = currentNumbers[index].ToString();
    }

    public void CheckPuzzle()
    {
        int j = 0;
        for (int i = 0; i < correctCombination.Length; i++)
        {
            if (currentNumbers[i] != correctCombination[i])
            {
                UIHints.Instance.ShowMessage("The code was incorrect.", 3f);
                lockObj.GetComponent<AudioSource>().clip = incorrectSFX;
                lockObj.GetComponent<AudioSource>().Play();
                return;
            }
            else
            {
                j++;
            }
        }

        if(j == correctCombination.Length)
        {
            PuzzleComplete();
        }
    }

    public void PuzzleComplete()
    {
        UIHints.Instance.ShowMessage("The code was correct! It should be unlocked now.", 4f);
        lockedDoor.GetComponent<BoxCollider>().enabled = true;
        lockCollider.SetActive(false);

        lockObj.GetComponent<AudioSource>().clip = correctSFX;
        lockObj.GetComponent<AudioSource>().Play();

        canvas.SetActive(false);
        lockObj.GetComponent<MeshRenderer>().enabled = false;

        gameUI.ExitView(1);
        playerButton.onClick.RemoveAllListeners();

    }
}
