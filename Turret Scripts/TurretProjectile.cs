using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float speed;
    [SerializeField] private int projectileDamage;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rb.MovePosition(transform.TransformPoint(speed * Time.deltaTime * Vector3.right));
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerCombat>().TakeDamage(projectileDamage);
            var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<DamageNumbers>().damageNumber = projectileDamage;
            Destroy(this.gameObject);
        }
    }
}
