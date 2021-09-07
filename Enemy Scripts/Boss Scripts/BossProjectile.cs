using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed;
    [SerializeField] private int projectileDamage;
    public GameObject damageNumbers;
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
            int calcTotalDefense = Mathf.RoundToInt(Character.MyInstance.Defense.Value + Character.MyInstance.Defense.BaseValue);
            int calcDefense = Mathf.RoundToInt(calcTotalDefense * .5f);
            int totalDamage = Random.Range(projectileDamage - calcDefense, projectileDamage * 2 - calcDefense);
            if (totalDamage <= 0)
            {
                totalDamage = 0;
                var missclone = Instantiate(damageNumbers, Character.MyInstance.transform.position, Quaternion.Euler(Vector3.zero));
                missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
                Destroy(this.gameObject);
            }
            else if (totalDamage > 0)
            {
                other.gameObject.GetComponent<PlayerCombat>().TakeDamage(totalDamage);
                var clone = Instantiate(damageNumbers, Character.MyInstance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
                Destroy(this.gameObject);
            }
        }
    }
}
