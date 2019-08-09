using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager Instance;

    void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameObject player;

    public void KillPlayer()
    {
        // player death animation, death screen and penalties
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
