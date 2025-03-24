using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject canvas;

    [Header("Used Objects")]
    public GameObject lockObj;
    public GameObject lockCam;
    public CinemachineVirtualCamera VirtualCamera;

    void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        canvas = GameObject.Find("Canvas_CombinationLock");
        canvas.SetActive(false);
        lockObj.SetActive(false);

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

    private void Update()
    {
        if (lockCam.transform.position == VirtualCamera.transform.position)
        {
            canvas.SetActive(true);
            lockObj.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
            lockObj.SetActive(false);
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
                Debug.Log("Incorrect Code!");
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
        Debug.Log("Puzzle Complete");
    }
}
