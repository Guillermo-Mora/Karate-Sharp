using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //Input handler del player "Karateka".
    // En este script también se gestiona la activación/desactivación del diálogo activo al pulsar
    // la tecla de interacció.

    [SerializeField] private PlayerController _characterController;
    [SerializeField] private GameObject pauseCanvas;
    private PlayerStats playerStats;

    private DialogHandler currentDialog;
    private LevelExit levelExit;
    private InputAction _moveAction, _jumpAction, _fightAction, _interactAction, _pauseAction;
  
    private bool wantJump = false;
    private bool wantFight = false;
    private bool pause = false;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        pauseCanvas.SetActive(false);
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _fightAction = InputSystem.actions.FindAction("Attack");
        _interactAction = InputSystem.actions.FindAction("Interact");
        _pauseAction = InputSystem.actions.FindAction("Pause");
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerStats.Died) return;

        if (_pauseAction.WasPressedThisFrame() && !pause) {
            pause = true;
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
            return;
        }

        if (_pauseAction.WasPressedThisFrame() && pause) {
            pause = false;
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
            return;
        }

        if (pause) return;

        if (_characterController.IsAttacking) return;
        
        Vector2 movementVector = _moveAction.ReadValue<Vector2>();

        movementVector.y = 0f;

        _characterController.Move(movementVector);

        if (_jumpAction.WasPressedThisFrame())
        {
            wantJump = true;
        }

        if (_fightAction.WasPressedThisFrame())
        {
            wantFight = true;
        }

        if (transform.position.y < -3) {
            transform.position = new Vector3(30, 0, 23);
        }

        if (_interactAction.WasPressedThisFrame() && levelExit != null)
        {
            Debug.Log("click detectado");
            levelExit.ActivateDialog();
        }

        if (_interactAction.WasPressedThisFrame() && currentDialog != null)
        {
            Debug.Log("click detectado");
            currentDialog.ActivateDialog();
        }
    }

    private void FixedUpdate()
    {
        if (wantJump)
        {
            _characterController.Jump();
            wantJump = false;
        }

        if (wantFight)
        {
            _characterController.Fight();
            wantFight = false;
        }
    }

    public void SetCurrentDialog(DialogHandler dialog)
    {
        Debug.Log("dialog asignado");
        currentDialog = dialog;
    }

    public void ClearDialog()
    {
        currentDialog = null;
    }


    public void SetLevelExit(LevelExit setLevelExit)
    {
        Debug.Log("dialog de level");
        levelExit = setLevelExit;
    }

    public void ClearLevelExit()
    {
        levelExit = null;
    }
}