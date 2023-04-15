using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int score = 1;
    public AudioSource collectionSound;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered: " + other.transform.tag);
        if (other.transform.CompareTag("Player"))
        {
            if (collectionSound) collectionSound.Play();
            ScoringSystem.score += score;
            Destroy(gameObject);
        }
    }
}
