using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public static int score = 0;
    public TMP_Text scoreDisplay;

    void Update()
    {
        scoreDisplay.text = score + "";
    }
}