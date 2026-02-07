using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    public UnityEvent onScoreChanged; // Event triggered when score changes
    public int score { get; private set; }

    public void AddScore(int amount)
    {
        score += amount;
        onScoreChanged.Invoke(); // Notify listeners of score change
    }
}
