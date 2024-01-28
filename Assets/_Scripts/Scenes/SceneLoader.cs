using _Scripts.GameState;
using _Scripts.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R) && !GameManager.Instance.IsInputBlocked)
            {
                var activeScene = SceneManager.GetActiveScene();
                MusicPlayer.Instance.Restart();
                SceneManager.LoadScene(activeScene.name);
            }
        }
    }
}