using System.Collections.Generic;
using UnityEngine;

public class PuzzleRegistry : MonoBehaviour
{
    public static PuzzleRegistry Instance;
    public AudioClip correctSFX;
    public AudioSource playerSource;

    private Dictionary<string, IPuzzle> puzzles = new Dictionary<string, IPuzzle>();

    public int puzzleCounter = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPuzzle(string id, IPuzzle puzzle)
    {
        if (!puzzles.ContainsKey(id))
        {
            puzzles.Add(id, puzzle);
        }
    }

    public void CheckPuzzleByID(string id)
    {
        if (puzzles.TryGetValue(id, out IPuzzle puzzle))
        {
            puzzle.CheckPuzzle();
        }
    }

    public void PuzzleFinished()
    {
        playerSource.clip = correctSFX;
        playerSource.Play();
        puzzleCounter++;
    }
}
