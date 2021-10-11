using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
public class CurrencyManager : NetworkBehaviour
{
    private int amount;
    public int minAmount;
    public int maxAmount;
    private Character thePlayer;

    public override void OnStartServer()
    {
        base.OnStartServer();
        amount = Random.Range(minAmount, maxAmount);
        //Debug.Log("coin start");
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
