using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHook : MonoBehaviour
{
    [SerializeField] private string sceneName; // Название сцены для загрузки

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
