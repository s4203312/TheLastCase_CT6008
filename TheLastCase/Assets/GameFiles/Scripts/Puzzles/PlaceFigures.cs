using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceFigures : MonoBehaviour
{
    public ManagerFiguresPuzzle puzzleManager;

    [SerializeField] private string correctFigureHead;
    [SerializeField] private Button placeFiguresButton;
    private bool figureCorrect = false;

    private void Start() {
        placeFiguresButton.gameObject.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player") && !figureCorrect) {
            placeFiguresButton.gameObject.SetActive(true);
        }
    }

    public void OnCollisionExit(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            placeFiguresButton.gameObject.SetActive(false);
        }
    }

    public void PlaceFigureButton() {

        // if inventory check returns true then
        figureCorrect = true;
        transform.parent.GetChild(1).gameObject.SetActive(true);
        puzzleManager.correctFigureHeadsPlaced++;
        placeFiguresButton.gameObject.SetActive(false);

        // else
        // print dialogue saying something like "Hmm it doesnt fit there" or "You dont seem to have something that goes there"
    }
}
