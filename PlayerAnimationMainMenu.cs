using UnityEngine;

public class PlayerAnimationMainMenu : MonoBehaviour
{
    //Animación que sale al inicio del juego en la escena de "MainMenu".
    //La animación se ejecuta al detectar que el usuario presiona la tecla "Enter"
    //El Script de "MMInputHandler" llama a estos métodos.
    
    [SerializeField] private Animator _animator;
    private Rigidbody rigidBody;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void StartAnimation() {
        _animator.SetBool("Crouch", false);
        _animator.SetBool("Jump", true);
        Invoke("FallAnimation", 0.4f);
    }
    
    private void FallAnimation() {
        _animator.SetBool("Jump", false);
        _animator.SetFloat("Fall", 1);
        rigidBody.isKinematic = false;
        rigidBody.AddForce((Vector3.up) * 200f, ForceMode.Impulse);
        rigidBody.AddForce((Vector3.right) * 700f, ForceMode.Impulse);
    }
}
