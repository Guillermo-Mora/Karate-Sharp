using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    //Script para las stats del jugador y el nivel que está jugando.
    //Muestra las stats del jugador en ese nivel en concreto
    // (Vidas, tiempo restante del nivel, heroes rescatados, monedas, etc).

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private TextMeshProUGUI peopleRescued;

    [SerializeField] private Image heartFilled_1;
    [SerializeField] private Image heartFilled_2;

    [SerializeField] private Image messi_img;
    [SerializeField] private Image sportacus_img;
    [SerializeField] private Image torvalds_img;

    [SerializeField] private Image fireImage;
    [SerializeField] private Animator fireImageAnimator;

    [SerializeField] private LevelTime levelTime;

    private void OnEnable()
    {
        playerStats.OnHealthChanged += UpdateHearts;
        playerStats.OnLivesChanged += UpdateLives;
        playerStats.OnMoneyChanged += UpdateMoney;
        playerStats.OnPeopleRescuedChanged += UpdatePeopleRescued;

        levelTime.OnTimeChanged += UpdateTime;
        levelTime.OnTimesUp += FireAnimation;
    }

    private void OnDisable()
    {
        playerStats.OnHealthChanged -= UpdateHearts;
        playerStats.OnLivesChanged -= UpdateLives;
        playerStats.OnMoneyChanged -= UpdateMoney;
        playerStats.OnPeopleRescuedChanged -= UpdatePeopleRescued;

        levelTime.OnTimeChanged -= UpdateTime;
        levelTime.OnTimesUp -= FireAnimation;
    }

    private void Start()
    {
        UpdateHearts();
        UpdateLives();
        UpdateMoney();
        UpdatePeopleRescued(3);
    }

    private void UpdateHearts()
    {
        heartFilled_1.enabled = playerStats.Health >= 1;
        heartFilled_2.enabled = playerStats.Health >= 2;
    }

    private void UpdateLives()
    {
        lives.text = playerStats.Lives.ToString();
    }

    private void UpdateMoney()
    {
        money.text = playerStats.Money.ToString();
    }

    private void UpdateTime() {
        time.text = levelTime.Time.ToString();
    }

    private void FireAnimation() {
        fireImage.enabled = true;
        fireImageAnimator.SetBool("fire", true);
        Invoke("EndFireAnimation", 1.5f);
    }

    private void EndFireAnimation() {
        fireImageAnimator.SetBool("fire", false);
        fireImage.enabled = false;
    }

    private void UpdatePeopleRescued(int id)
    {
        peopleRescued.text = playerStats.PeopleRescuedCount + "/" + playerStats.TotalPeopleToRescue;
        
        switch(id) {
        case 0:
            messi_img.enabled = true;
            break;
        case 1:
            sportacus_img.enabled = true;
            break;
        case 2:
            torvalds_img.enabled = true;
            break;
        case 3:
            messi_img.enabled = false;
            sportacus_img.enabled = false;
            torvalds_img.enabled = false;
            break;
        }
    }
}