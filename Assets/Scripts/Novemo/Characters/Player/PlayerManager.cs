using UnityEngine;
using UnityEngine.SceneManagement;

namespace Novemo.Characters.Player
{
    public class PlayerManager : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// A singleton instance of a PlayerManager script
        /// </summary>
        public static PlayerManager Instance;

        void Awake()
        {
            Instance = this;
        }

        #endregion

        /// <summary>
        /// Player object with all necessary scripts attached
        /// </summary>
        public GameObject player;

        public GameObject uiCanvas;

        public void KillPlayer()
        {
            // player death animation, death screen and penalties
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
