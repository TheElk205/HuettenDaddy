using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class StartMenuBehaviour : MonoBehaviour
{
    private bool enabled = true;

    // Update is called once per frame
    void Update()
    {
        if (GameEventSystem.currentGameState != GameState.StartMenu && enabled)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            enabled = false;
        }
        
        if (GameEventSystem.currentGameState == GameState.StartMenu && !enabled)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

            enabled = true;
        }
    }
}
