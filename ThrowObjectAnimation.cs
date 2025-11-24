using UnityEngine;

public class ThrowObjectAnimation : MonoBehaviour
{
    //Animación para objetos destruibles por el "Karateka".

    private Rigidbody rigidbody;
    private Collider collider;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void ThrowAndDestroyObject() {
        Debug.Log("Empeiza animacion de destruir");
        collider.enabled = false;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.AddForce((Vector3.forward) * -30f, ForceMode.Impulse);
        rigidbody.AddForce((Vector3.right) * 10f, ForceMode.Impulse);
        rigidbody.AddTorque((Vector3.forward) * -20f, ForceMode.Impulse);
        Invoke("DestroyParentGameObject", 1.5f);
    }

    private void DestroyParentGameObject() {
        Destroy(transform.parent.gameObject);
        Debug.Log("GO padre destruido");
    }
}
