using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class Collectible : MonoBehaviour
{
    public int score = 1;
    public AudioSource collectionSound;
    private bool playerHere = false;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered: " + other.transform.tag);
        if (other.transform.CompareTag("Player"))
        {
            playerHere = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Left: " + other.transform.tag);
        if (other.transform.CompareTag("Player"))
        {
            playerHere = false;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (!playerHere) return;
        var inputAction = other.GetComponent<PlayerInput>().actions["Action"];
        Debug.Log(inputAction.WasPerformedThisFrame());
        Debug.Log(inputAction.IsPressed());
            
        if (inputAction.IsPressed())
        {
            if (collectionSound) collectionSound.Play();
            ScoringSystem.score += score;
            Destroy(gameObject);
        }
    }
}
