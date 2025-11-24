using UnityEngine;

public class SceneGMDebugger : MonoBehaviour
{
   [SerializeField] private GameObject gameManagerPrefab;

    void Awake()
    {
        //Este script lo uso en todas las escenas para poder iniciarlas y testear sin problemas,
        // ya que por defecto sólo tendría el GameManager disponible si inicio el juego
        // desde la escena de ("InitScene"), que es cuándo se instancia.
        if (GameManager.Instance == null) Instantiate(gameManagerPrefab);
    }
}
