using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public RawImage gameOverImage;
    // Start is called before the first frame update
    void Start()
    {
        gameOverImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        gameOverImage.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (ScoringSystem.score < 0)
        {
            gameOverImage.gameObject.SetActive(true);
            GameEventSystem.currentGameState = GameState.GameOver;
        }

        if (GameEventSystem.currentGameState == GameState.GameOver)
        {
            if(Input.GetKeyDown (KeyCode.Return))
            {
                gameOverImage.gameObject.SetActive(false);
                GameEventSystem.currentGameState = GameState.StartMenu;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            }
        }
    }
}
