using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Propiedad para almacenar los componentes transform de:
    // - GameObject al que seguirá la cámara
    // - GameObject al que enfocará la cámara (personaje jugador)
    [SerializeField] private Transform _followTarget, _lookTarget;

    // Método de MonoBehaviour que se ejecuta en cada frame tras el resto de todos los métodos Update
    private void LateUpdate()
    {
        // Seguimiento
        transform.position = _followTarget.position;
        transform.LookAt(_lookTarget);
    }
}