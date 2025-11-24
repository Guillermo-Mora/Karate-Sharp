using UnityEngine;

public class ChangeSceneCollider : MonoBehaviour
{
    private SceneController sceneController;
    [SerializeField] private string scene;
    private bool playerAlreadyDetected = false;

    void Start() {
        sceneController = Object.FindAnyObjectByType<SceneController>();
    }
    
    //Lo necesito para poder cambiar de escena en el tutorial al tirarse el jugador al boxCollider
    // que está en el vacío.
    void OnTriggerEnter(Collider other) {
        if (playerAlreadyDetected) return;

        if (other.CompareTag("Character")) {
            playerAlreadyDetected = true;
            Debug.Log("detectada collision del jugador");
            sceneController.startScene(scene);
        }
    }
}
