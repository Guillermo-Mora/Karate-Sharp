using UnityEngine;
using TMPro;

public class LevelExit : MonoBehaviour
{
    //Este script gestiona la interacción con el diálogo de salida del nivel.
    //El cuál debe comprobar si el jugador ha rescatado a todos los héroes.

    //No he reciclado el script de DialogHandler para esto, ya que la manera en la que se
    // activa/desactiva el mensaje al pulsar la tecla de interacción es diferente.
    
    [SerializeField] private SpriteRenderer keypressWhite;
    [SerializeField] private GameObject visibleDialog;
    [SerializeField] private GameObject lockedDialog;
    [SerializeField] private PlayerStats playerStats;
    private bool dialogActivated;

    void Awake() {

        visibleDialog.SetActive(false);
        lockedDialog.SetActive(false);
    }

   private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Visor")) {
                lockedDialog.SetActive(true);
                InvokeRepeating("KeyPressBlinkingAnimation",0f, 0.5f);
            }
            other.GetComponentInParent<InputHandler>()?.SetLevelExit(this);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Visor")) {
            CancelInvoke("KeyPressBlinkingAnimation");
            lockedDialog.SetActive(false);
            visibleDialog.SetActive(false);
        }
            other.GetComponentInParent<InputHandler>()?.ClearLevelExit();
    }

    public void ActivateDialog() {
        CancelInvoke("KeyPressBlinkingAnimation");
        lockedDialog.SetActive(false);

        if (playerStats.AreAllPeopleRescued()) {
            playerStats.WinGame();
        } else {
            visibleDialog.SetActive(true);
        }
    }

    private void KeyPressBlinkingAnimation() {
        keypressWhite.enabled = !keypressWhite.enabled;
    }
}

