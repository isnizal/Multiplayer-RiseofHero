using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CurrencyManager : MonoBehaviour
{
    private int amount;
    public int minAmount;
    public int maxAmount;
    private Character thePlayer;

    void Start()
    {
        amount = Random.Range(minAmount, maxAmount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                return;
            thePlayer = other.gameObject.GetComponent<Character>();
            thePlayer.AddCurrency(amount);
            SoundManager.PlaySound(SoundManager.Sound.PickupCurrency);
            thePlayer.GetComponent<PlayerCombat>().DisplayCoin(amount);
            thePlayer.GetComponent<PlayerCombat>().CmdDestroyObjects(this.gameObject); 
        }
    }

}
