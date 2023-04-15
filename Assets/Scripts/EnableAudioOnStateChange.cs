using DefaultNamespace;
using UnityEngine;

public class EnableAudioOnStateChange : MonoBehaviour
{
    private AudioSource audioSource;

    public GameState enableOnStateChange = GameState.Playing;
    
    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEventSystem.currentGameState == enableOnStateChange && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (GameEventSystem.currentGameState != enableOnStateChange && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
