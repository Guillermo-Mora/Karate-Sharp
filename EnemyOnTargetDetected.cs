using UnityEngine;

public class EnemyOnTargetDetected : MonoBehaviour
{
    [SerializeField] private ModularEnemyMovement enemyScript;
    private bool targetDetected = false;

    //Lo uso en el "Enemy Sumo". Tiene un boxCollider con isTrigger a su alrededor.
    // Al entrar el jugador en esa área, comenzará a perseguirlo.
    void OnTriggerEnter(Collider other) {
        if (targetDetected) return;

        //Detecto el visor del jugador, ya que su forma es rectangular y uniforme
        //Como en el caso de la detección para los diálogos, que también uso el visor.
        if (other.CompareTag("Visor")) {
            targetDetected = true;
            enemyScript.ChaseTarget();
        }
    }
}
