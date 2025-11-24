public interface IDamageDealer
{
    //Interfaz a implementar por enemigos y spikes
    int DamageAmount { get; }
    bool CausesKnockback { get; }
    bool KillableEnemy {get; }
}
