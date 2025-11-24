using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MMInputHandler : MonoBehaviour
{
    //Input Handler que uso para todos los menús en escenas de menús ("MainMenu", "GameOver", "WinScene").
    //En mi caso, todos los menús que he hecho sólo necesitan detectar la tecla enter.

    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private PlayerAnimationMainMenu playerAnimationMainMenu;
    private SceneController sceneController;
    [SerializeField] private string scene;

    private InputAction _startAction;
    private bool gameStart = false;

    void Start()
    {
        sceneController = UnityEngine.Object.FindAnyObjectByType<SceneController>();
        _startAction = InputSystem.actions.FindAction("Start");
        Cursor.visible = false;
        InvokeRepeating("BlinkingAnimation", 0f, 0.8f);
    }

    void Update() {
        if (gameStart) return;

        if (_startAction.WasPressedThisFrame())
        {
            gameStart = true;
            CancelInvoke("BlinkingAnimation");
            startText.enabled = false;

            //Debo realizar esta comprobación, ya que este mismo script lo reciclo para los menús de
            // las siguientes escenas ("MainMenu", "GameOver", "WinScene").
            // "GameOver" y "WinScene" No tienen esta animación.
            if (playerAnimationMainMenu != null) playerAnimationMainMenu.StartAnimation();
            sceneController.startScene(scene);
        }
    }

    private void BlinkingAnimation() {
        startText.enabled = !startText.enabled;
    }
}
