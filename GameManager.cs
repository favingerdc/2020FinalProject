using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
bool GameHasEnded = false;

public float restartDelay = 1f;

public int objCount = 0;

public int objMax = 4;

public void CompleteLevel ()
{
    if (objCount == objMax - 1)
    {
    Debug.Log("Game Won!");
    //EndGame();
    }
    else
    {
        objCount++;
    }
}

public void EndGame()
{
    if (GameHasEnded == false)
    {
      GameHasEnded = true;
      Debug.Log("Game Over!");
      Invoke ("Restart",restartDelay); 
    }
}

void Restart()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}
