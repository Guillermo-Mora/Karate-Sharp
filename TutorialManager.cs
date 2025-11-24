using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //Script sólo usado en el tutorial. Cambia el modo del jugador "Karateka" a tutorial = true.
    // Para evitar perder vida, morir, etc.

    [SerializeField] private PlayerStats playerStats;
    
    void Start()
    {
        playerStats.tutorialMode();
        Debug.Log("modo tutorial sin recibir daño, solo animaciones");
    }
}
