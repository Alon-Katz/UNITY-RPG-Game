using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject player;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("PlayerManager is singleton!");
            return;
        }
        instance = this;
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
