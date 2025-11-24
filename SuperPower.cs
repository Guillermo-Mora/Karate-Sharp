using UnityEngine;

public class SuperPower : MonoBehaviour
{
    //Script que instancia los gameObjects "shurikens" que puede lanzar el "Karateka" cuándo tiene
    // el super poder activado.

    // El encargado de gestionar lo que le ocurre a este gameObject al chocar
    // es el script de "PlayerAtackCollisionHandler"

    [SerializeField] private GameObject shuriken;
    [SerializeField] private float throwForce = 20f;

    public void ThrowShuriken(bool leftThrow) {
        Debug.Log("Lanzo el shuriken");
        GameObject newShuriken = Instantiate(shuriken, transform.position, transform.rotation);
        Rigidbody rigidbody = newShuriken.GetComponent<Rigidbody>();
        Vector3 direction = leftThrow ? -transform.right : transform.right;
        rigidbody.AddForce(direction * throwForce, ForceMode.VelocityChange);
        Destroy(newShuriken, 1f);
    }
}
