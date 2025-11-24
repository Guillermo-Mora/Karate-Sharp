using UnityEngine;

public class DeathVoidCollider : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private bool playerAlreadyDetected = false;
    
    //Lo uso en el nivel para detectar al jugador caer el vacío (chocar con boxCollider isTrigger).

    //Lo he hecho así ya que en mi experienia me ha dado mucha más consistencia y menos problemas
    //que capturando la muerte por caída según la posición del jugador en el eje y.
    void OnTriggerEnter(Collider other) {
        if (playerAlreadyDetected) return;

        if (other.CompareTag("Character")) {
            playerAlreadyDetected = true;
            Debug.Log("detectada collision del jugador cayendo al vacío");
            playerStats.LoseLife();
            playerAlreadyDetected = false;
        }
    }
}
