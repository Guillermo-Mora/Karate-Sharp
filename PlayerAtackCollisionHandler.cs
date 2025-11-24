using UnityEngine;

public class PlayerAtackCollisionHandler : MonoBehaviour
{
    //Este script lo uso tanto para la hitBox de ataque normal del "Karateka" como para
    // los shurikens que puede lanzar cuando tiene el super poder activado.

    //Al tener el shuriken este mismo script para realizar daño,
    // siempre deberá de destruirse al chocar con cualquier cosa.

    void OnTriggerEnter(Collider other) {
        GameObject obj = other.gameObject;
        Debug.Log("Colision con" + obj.name + " con tag " + obj.tag);

        if (obj.TryGetComponent<IDamageDealer>(out var dealer) && dealer.KillableEnemy) {
            Debug.Log("Es enemigo matable");
            obj.GetComponentInParent<ModularEnemyMovement>().DieSequence();
        }

        if (obj.CompareTag("WoodLog")) {
            Debug.Log("Es objeto destruible");
            obj.GetComponent<ThrowObjectAnimation>().ThrowAndDestroyObject();
        }

        //Destruyo el game object AL FINAL tras chocar si es un shuriken.
        if (gameObject.CompareTag("shuriken")) {
            Destroy(gameObject);
        }
    }
}
