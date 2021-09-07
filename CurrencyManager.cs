using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int amount;
    public int minAmount;
    public int maxAmount;
    private Character thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<Character>();
        amount = Random.Range(minAmount, maxAmount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thePlayer.AddCurrency(amount);
            SoundManager.PlaySound(SoundManager.Sound.PickupCurrency);
            PlayerCombat.CombatInstance.DisplayCoin(amount);
            Destroy(this.gameObject);
        }
    }
}
