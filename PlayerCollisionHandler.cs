using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    //Script que gestiona lo que le ocurre al jugador "Karateka" al chocar con diferentes objetos.

    private PlayerStats playerStats;
    private PlayerController playerController;

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
    }

    //Colisiones fisicas con boxColliders sin IsTrigger = true (Pinchos, Trampolines)
    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (playerStats.Died) return;

        GameObject obj = hit.gameObject;

        if (obj.TryGetComponent<IDamageDealer>(out var dealer)) {
            playerStats.LoseHealth(dealer.DamageAmount);

            if (dealer.CausesKnockback) {
                Vector3 hitDirection = (transform.position - obj.transform.position).normalized;
                playerController.KnockbackPlayer(hitDirection);
            }
        }

        if (obj.CompareTag("Trampoline")) {
            playerController.TrampolineJumpPlayer(60f);
        }

        if (obj.CompareTag("SuperTrampoline")) {
            playerController.TrampolineJumpPlayer(160f);
        }
    }

    //Colisiones con boxColliders con IsTrigger = true (Monedas, Vidas, Enemigos)
    void OnTriggerEnter(Collider other) {
        if (playerStats.Died) return;

        GameObject obj = other.gameObject;

        if (obj.TryGetComponent<IHealthHealer>(out var healer)) {
            playerStats.HealHealth(healer.HealAmount);
            Destroy(obj.transform.parent.gameObject);
            return;
        }

        if (obj.TryGetComponent<IMoneySource>(out var money)) {
            playerStats.GetMoney(money.MoneyAmount);
            Destroy(obj.transform.parent.gameObject);
            return;
        }

        if (obj.CompareTag("SuperPowerItem")) {
            Debug.Log("Super power item");
            playerStats.ActivateSuperPower();
            Destroy(obj.transform.parent.gameObject);
            return;
        }

        if (obj.TryGetComponent<IDamageDealer>(out var dealer)) {
            playerStats.LoseHealth(dealer.DamageAmount);

            if (dealer.CausesKnockback) {
                Vector3 hitDirection = (transform.position - obj.transform.position).normalized;
                playerController.KnockbackPlayer(hitDirection);
            }
        }
    }
}
