using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI instance;
    public static EnemyAI EnemyAIInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyAI>();
            }
            return instance;
        }

    }
    public Transform target;
    public float moveSpeed;
    public float chaseRadius;
    public float attackRadius;

    private Rigidbody2D myRigidbody;
    private bool moving;
    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;
    //public GameObject damageNumbers;

    //public GameObject enemyText;

    private Animator _animator;
    private GameObject thePlayer;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        thePlayer = GameObject.FindWithTag("Player");
        if (thePlayer != null)
            target = thePlayer.transform;

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

        //var nameTag = Instantiate(enemyText);
        //nameTag.GetComponentInChildren<Transform>().transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        //nameTag.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<EnemyStats>().enemyName.ToString();
        //nameTag.transform.SetParent(transform);
    }
    public bool freezing = false;
    private void Update()
    {
        if (!freezing)
        {
            EnemyFollow();
            if (myRigidbody.velocity != Vector2.zero)
            {
                _animator.SetFloat("moveX", myRigidbody.velocity.x);
                _animator.SetFloat("moveY", myRigidbody.velocity.y);
                _animator.SetBool("moving", true);
            }
            else
            {
                _animator.SetBool("moving", false);
            }
        }
    }

     IEnumerator FreezeCooldown(float value)
    {
        GetComponent<EnemyStats>().isAttack = false;
        yield return new WaitForSeconds(value);
        freezing = false;
        GetComponent<SpriteRenderer>().color = Color.white;

    }
    public void StartFreeze(float value)
    {
        StartCoroutine(FreezeCooldown(value));
    }
    void EnemyFollow()
    {
        if (target == null)
            return;

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            FreeRoam();
        }
    }

    void FreeRoam()
    {
        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = moveDirection;
            if (timeToMoveCounter < 0f)
            {
                moving = false;
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;
            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            }
        }
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
	}

	//private void OnTriggerEnter2D(Collider2D other)
	//{
	//    if (other.gameObject.CompareTag("Player") && !other.isTrigger)
	//    {
	//        if (attackRadius == 1)
	//        {
	//            int enemyDamage = GetComponent<EnemyStats>().enemyAttackPower;
	//            other.gameObject.GetComponent<PlayerCombat>().DamageToPlayer(enemyDamage);
	//            var clone = Instantiate(damageNumbers, thePlayer.transform.position, Quaternion.Euler(Vector3.zero));
	//            clone.GetComponent<DamageNumbers>().damageNumber = enemyDamage;
	//        }
	//    }
	//}
}
