using UnityEngine;

public class InitScene : MonoBehaviour
{
    //Script inicial presente en la escena "InitScene", escena en la cual instancio el GameManager,
    // que usaré durante todo el resto del juego.

    private SceneController sceneController;

    void Start()
    {
        sceneController = Object.FindAnyObjectByType<SceneController>();
        Invoke("StartGame", 0.5f);
    }

    private void StartGame() {
        sceneController.startScene("MainMenu");
    }
}
