using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDamages : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<PlayerCombat>().MeleeAttack(collision);
        //PlayerCombat.CombatInstance.MeleeAttack(collision);
    }

}
