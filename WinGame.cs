using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
public GameManager gameManager;

void OnTriggerEnter()
{
    Destroy(gameObject);
    gameManager.CompleteLevel();
}
}
