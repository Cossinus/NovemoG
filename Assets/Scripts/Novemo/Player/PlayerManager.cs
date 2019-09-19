using UnityEngine;
using UnityEngine.SceneManagement;

namespace Novemo.Player
{
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
}
