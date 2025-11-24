using UnityEngine;

public class Money : MonoBehaviour, IMoneySource
{
    //Dinero que da el coleccionable de coin
    [SerializeField] private int money = 1;
     public int MoneyAmount => money;
}
