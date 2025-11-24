using UnityEngine;

public class EnemyAtack : MonoBehaviour, IDamageDealer
{
   //Propiedades del enemigo
   // No killables (Slimes, Cars y Spikes)
   // No knockback (Spikes)
   // Damage 1 (All)
   [SerializeField] private int damage = 1;
   [SerializeField] bool causesKnockback = true;
   [SerializeField] bool isKillableEnemy = false;

   //Getters del enemigo
   public int DamageAmount => damage;
   public bool CausesKnockback => causesKnockback;
   public bool KillableEnemy => isKillableEnemy;
}
