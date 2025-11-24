using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake() {
        // Contiene el Event System y Scene Controller. Lo necesito en todas las escenas y sin duplicados.

        // Si no existe (== null), lo instancio desde "SceneGMDebugger". Esto me sirve para poder testear directamente escenas que no lo tengan sin problemas,
        // ya que sólo se instancia en la primera escena de iniciación del juego "InitScene".
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);


// Aquí debajo declaro normas de ignorar ciertas colisiones entre capas:


// Para evitar colisiones entre characterControllers, que provoca que crashee Unity (Jugador y enemigos).
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Enemy"),
            LayerMask.NameToLayer("Player"),
            true);

// Estas dos sirven para evitar que el boxCollider de ataque del jugador detecte
// al propio jugador o al visor del jugador, evitando que el ataque pueda tener
// efecto en enemigos y objetos destruibles.
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Visor"),
            LayerMask.NameToLayer("Atack-HitBox"),
            true);

        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Atack-HitBox"),
            true);

//Esta para evitar que el ataque del jugador pille el characterController del enemigo, en vez de su boxCollider.
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Enemy"),
            LayerMask.NameToLayer("Atack-HitBox"),
            true);



//Aquí abajo declaro las normas de colisiones para los Shuriken lanzables por el jugador:
//No debe poder colisionar con character controllers de enemigos. En su lugar, colisionará con sus boxColliders.
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Enemy"),
            LayerMask.NameToLayer("Attack-Shuriken"),
            true);

//No debe poder colisionar con el visor del propio jugador
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Visor"),
            LayerMask.NameToLayer("Attack-Shuriken"),
            true);

//No debe poder colisionar con el propio jugador
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Attack-Shuriken"),
            true);

//Monedas y powerups con el shuriken (Coleccionables)
//Importante para evitar que en una pelea los shurikens se destruyan al tocar power-ups o monedas.
        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("Collectable"),
            LayerMask.NameToLayer("Attack-Shuriken"),
            true);
    }
}
