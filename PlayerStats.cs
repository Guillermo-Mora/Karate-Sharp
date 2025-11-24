using System;
using UnityEngine;
using System.Linq;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    //Script encargado de gestionar todas las stats y estados del jugador.

    private SceneController sceneController;
    [SerializeField] private GameObject dieCanvas;
    [SerializeField] private GameObject onScreenStatsCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject character;
    [SerializeField] private Animator _animator;
    [SerializeField] private LevelTime levelTime;
    [SerializeField] private Color ninjaColor = new Vector4(0.9f, 0.0f, 0.0f, 0.1f);
    [SerializeField] private Color healHealthColor = new Vector4(0.9f, 0.0f, 0.0f, 0.1f);
    [SerializeField] private Color looseHealthColor = new Vector4(0.9f, 0.0f, 0.0f, 0.1f);

    public event Action OnHealthChanged;
    public event Action OnLivesChanged;
    public event Action OnMoneyChanged;
    public event Action<int> OnPeopleRescuedChanged;

    private int startingLives = 3;
    private int minLives = 1;
    private int maxHealth = 2;
    private int minHealth = 1;
    private int maxLives = 99;
    private int maxMoney = 99;
    private Vector3 startPosition = new Vector3(-755.7f, 24.87f, 13.33f);
    private Vector3 checkpintPosition;

    private int lives;
    private int health;
    private bool superPowerActive;
    private int money;
    private bool checkpointReached;
    private bool[] peopleRescued;
    private bool isInvulnerable;
    private SpriteRenderer spriteRenderer;
    private bool tutorial;
    private bool died;
    private Color defaultColor;

    public int Health => health;
    public int Lives => lives;
    public int Money => money;
    public int PeopleRescuedCount => peopleRescued.Count(p => p);
    public int TotalPeopleToRescue => peopleRescued.Length;
    public bool Died => died;
    public bool IsInvulnerable => isInvulnerable;
    public bool SuperPowerActive => superPowerActive;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        died = false;
        tutorial = false;
        lives = startingLives;
        health = maxHealth;
        peopleRescued = new bool[3];
        DeactivateSuperPower();
        money = 0;
        checkpointReached = false;
        isInvulnerable = false;
    }

    private void Start()
    {
        //En el modo tutorial no los necesito
        if (!tutorial) {
            Debug.Log("Reposicion al jugador");
            RepositionPlayer();
            if (onScreenStatsCanvas != null) onScreenStatsCanvas.SetActive(true);
            if (dieCanvas != null) dieCanvas.SetActive(false);
        }
        _animator = GetComponentInChildren<Animator>();
        sceneController = UnityEngine.Object.FindAnyObjectByType<SceneController>();

        pauseCanvas.SetActive(false);
        OnHealthChanged?.Invoke();
        OnLivesChanged?.Invoke();
        OnMoneyChanged?.Invoke();
        OnPeopleRescuedChanged?.Invoke(3);
    }

    public void LoseLife() {
        Debug.Log("Desactivo al jugador");
        PlayerState(false);
        _animator.SetBool("Crouch", true);

        lives--;
        OnLivesChanged?.Invoke();
        DieAnimation();

        if (lives < minLives) {
            GameOver();
        } else {
            levelTime.restartTimerOnDeath();
            health = maxHealth;
            Debug.Log("Recargo salud tras morir");
            OnHealthChanged?.Invoke();
            DeactivateSuperPower();

            isInvulnerable = false;
        }
    }

    public void GainLife() {
        if (lives < maxLives) {
            lives++;
            OnLivesChanged?.Invoke();
        }
    }

    public void HealHealth(int healAmount) {
        if (health + healAmount <= maxHealth) {
            health += healAmount;
        } else if (health + healAmount > maxHealth) {
            health = maxHealth;
        }
        OnHealthChanged?.Invoke();
        isInvulnerable = true;
        InvulnerableSequence((superPowerActive ? ninjaColor : healHealthColor), (superPowerActive ? ninjaColor : defaultColor));
    }

    public void LoseHealth(int num) {
        if (isInvulnerable) return;

        if (superPowerActive) {
            DeactivateSuperPower();
        } else {
            health = health - num;
            OnHealthChanged?.Invoke();
        }

        isInvulnerable = true;

        if (health < minHealth && !tutorial) {
            LoseLife();
        }
        InvulnerableSequence(looseHealthColor, defaultColor);
    }

    public void GameOver() {
        sceneController.startScene("GameOver");
    }

    public void WinGame() {
        sceneController.startScene("WinScene");
    }

    private void EndInvulnerability() {
        isInvulnerable = false;
        CancelInvoke("BlinkingAnimation");
        spriteRenderer.enabled = true;
    }

    private void BlinkingAnimation() {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    IEnumerator SetEndColor(Color color) {
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.color = color;
    }

    private void DieAnimation() {
        StartCoroutine(DieSequence());
    }

    private IEnumerator DieSequence() {
        sceneController.outAnimation();
        yield return new WaitForSeconds(1f);

        dieCanvas.SetActive(true);
        sceneController.inAnimation();

        Debug.Log("Reposiciono al jugador");
        RepositionPlayer();

        yield return new WaitForSeconds(2f);
        sceneController.outAnimation();
        yield return new WaitForSeconds(1f);
        sceneController.inAnimation();

        dieCanvas.SetActive(false);

        Debug.Log("Reactivo al jugador");
        _animator.SetBool("Crouch", false);
        PlayerState(true);
    }

    private void PlayerState(bool state) {
        if (state) died = !state;

        character.GetComponent<CharacterController>().enabled = state;

        if (!state) died = !state;
    }

    public void RepositionPlayer() {
        character.transform.position = checkpointReached ? checkpintPosition : startPosition;
    }

    public void GetMoney(int moneyAmount) {
        if (money + moneyAmount > maxMoney) {
            GainLife();
            money = 0;
            OnMoneyChanged?.Invoke();
        } else {
            money += moneyAmount;
            OnMoneyChanged?.Invoke();
        }
    }

    public void tutorialMode() {
        tutorial = true;
    }

    public void GetCheckpoint(Vector3 position) {
        checkpintPosition = position;
        checkpintPosition.z = 13.33f;
        checkpointReached = true;
    }

    public void RescuePerson(int id) {
        peopleRescued[id] = true;
        OnPeopleRescuedChanged?.Invoke(id);
    }

    public void ActivateSuperPower() {
        Debug.Log("Activo super poder");
        superPowerActive = true;
        health = maxHealth;
        OnHealthChanged?.Invoke();

        isInvulnerable = true;
        InvulnerableSequence(ninjaColor, ninjaColor);
    }

    public void DeactivateSuperPower() {
        superPowerActive = false;
        spriteRenderer.color = defaultColor;
    }

    public bool AreAllPeopleRescued() {
        foreach (bool rescued in peopleRescued){
            if (!rescued) return false;
        }
        return true;
    }

    private void InvulnerableSequence(Color startColor, Color endColor) {
        spriteRenderer.color = startColor;
        InvokeRepeating("BlinkingAnimation", 0f, 0.15f);
        Invoke("EndInvulnerability", 1.5f);
        StartCoroutine(SetEndColor(endColor));
    }
}
