using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

    PlayerMovement _playerMovement;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                return;
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            other.gameObject.GetComponent<PlayerCombat>().TakeDamage(projectileDamage);
            int calcTotalDefense = Mathf.RoundToInt(_playerMovement.character.Defense.Value + _playerMovement.character.Defense.BaseValue);
            int calcDefense = Mathf.RoundToInt(calcTotalDefense * .5f);
            int totalDamage = Random.Range(projectileDamage - calcDefense, projectileDamage * 2 - calcDefense);
            if (totalDamage <= 0)
            {
                totalDamage = 0;
                var missclone = Instantiate(damageNumbers, _playerMovement.character.transform.position, Quaternion.Euler(Vector3.zero));
                missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
                Destroy(this.gameObject);
            }
            else if (totalDamage > 0)
            {
                other.gameObject.GetComponent<PlayerCombat>().TakeDamage(totalDamage);
                var clone = Instantiate(damageNumbers, _playerMovement.character.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
                Destroy(this.gameObject);
            }
        }
    }
}
