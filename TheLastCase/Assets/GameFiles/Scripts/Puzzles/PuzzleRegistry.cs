using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRegistry : MonoBehaviour
{
    public static PuzzleRegistry Instance;

    private Dictionary<string, IPuzzle> puzzles = new Dictionary<string, IPuzzle>();

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
        else
        {
            Debug.LogWarning($"Puzzle with ID {id} not found!");
        }
    }
}
