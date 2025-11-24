using UnityEngine;

public class Healer : MonoBehaviour,  IHealthHealer
{
    //Propiedades de items de regeneración de vida (Onigiris)
    [SerializeField] private int heal = 1;
     public int HealAmount => heal;
}
