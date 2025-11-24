using UnityEngine;
using TMPro;

public class PlayerDieHUD : MonoBehaviour
{
    //Script para el canvas de muerte, que muestra las vidas que le quedan al jugador.
    
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI lives;
    
    private void OnEnable() {
        lives.text = playerStats.Lives.ToString();
    }
}
