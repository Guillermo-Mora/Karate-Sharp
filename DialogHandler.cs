using UnityEngine;
using TMPro;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer keypressWhite;
    [SerializeField] private GameObject visibleDialog;
    [SerializeField] private GameObject lockedDialog;
    [SerializeField] private TextMeshPro characterName;
    [SerializeField] private SpriteRenderer rope;
    private PlayerStats playerStats;
    private bool dialogActivated;

    //Al poder haber varios diálogos en la escena (Como en "Level"). Para capturar cual debe
    // activarse en cada momento al pulsar la tecla de interacción,
    // me comunico con el InputHandler del jugador y le indico que diálogo es le que debe de activarse.
    // Y entonces el input handler toma ese diálogo y lo activa llamando a ActivateDialog();
    // Este mismo script lo reciclo para los checkpoints.
    void Awake() {
        dialogActivated = false;

        visibleDialog.SetActive(false);
        lockedDialog.SetActive(false);
        if (characterName != null) characterName.enabled = false;
    }

    void Start() {
        playerStats = UnityEngine.Object.FindAnyObjectByType<PlayerStats>();
    }

   private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Visor")) {
            Debug.Log("detectada colision del jugador");
            if (dialogActivated) {
                visibleDialog.SetActive(true);
            } else {
                lockedDialog.SetActive(true);
                InvokeRepeating("KeyPressBlinkingAnimation",0f, 0.5f);
            }
            if (characterName != null) characterName.enabled = true;
            other.GetComponentInParent<InputHandler>()?.SetCurrentDialog(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Visor")) {
            if (dialogActivated) {
                 visibleDialog.SetActive(false);
            } else {
                CancelInvoke("KeyPressBlinkingAnimation");
                lockedDialog.SetActive(false);
            }
            if (characterName != null) characterName.enabled = false;
            other.GetComponentInParent<InputHandler>()?.ClearDialog();
        }
    }

    public void ActivateDialog() {
        if (dialogActivated) return;
        CancelInvoke("KeyPressBlinkingAnimation");
        lockedDialog.SetActive(false);

        dialogActivated = true;
        visibleDialog.SetActive(true);

        switch (tag) {
            case "messi":
                playerStats.RescuePerson(0);
                rope.enabled = false;
                break;
            case "sportacus":
                playerStats.RescuePerson(1);
                rope.enabled = false;
                break;
            case "torvalds":
                playerStats.RescuePerson(2);
                rope.enabled = false;
                break;
            case "checkpoint":
                playerStats.GetCheckpoint(transform.position);
                break;
            default:
            break;
        }
    }

    private void KeyPressBlinkingAnimation() {
        keypressWhite.enabled = !keypressWhite.enabled;
    }
}
