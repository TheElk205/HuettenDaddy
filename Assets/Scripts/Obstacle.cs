using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int score = -1;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered: " + other.transform.tag);
        if (other.transform.CompareTag("Player"))
        {
            ScoringSystem.score += score;
        }
    }
}
