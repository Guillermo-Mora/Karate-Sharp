using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    //Plataforma móvil con dos puntos.

    //No lo he usado porque junto con el character controller del jugador, generaba animaciones buggeadas
    // y no puede arrastrarlo horizontalmente encima de la plataforma.
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [SerializeField] private float movementSpeed = 3f;
    private Vector3 platformDestiny;

    void Start() {
        platformDestiny = endTransform.position;
    }

    void Update() {
        if (transform.position == endTransform.position)
        platformDestiny = startTransform.position;
        else if (transform.position == startTransform.position)
        platformDestiny = endTransform.position;

        Vector3 newPosition = Vector3.MoveTowards(transform.position, platformDestiny, movementSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
