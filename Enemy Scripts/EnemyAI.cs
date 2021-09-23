using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using Mirror.Experimental;

[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(NetworkRigidbody2D))]
public class EnemyAI : NetworkBehaviour
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

    //private Rigidbody2D myRigidbody;
    private bool moving;
    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private Vector3 moveDirection;
    //public GameObject damageNumbers;

    //public GameObject enemyText;

    //private Animator _animator;

    private NetworkRigidbody2D _netRigidbody2D;
    private NetworkAnimator _netAnimator;
    //private GameObject thePlayer;

    private void InitializeEnemyStart()
    {
        _netRigidbody2D = GetComponent<NetworkRigidbody2D>();
        _netRigidbody2D.target = GetComponent<Rigidbody2D>();
        _netAnimator = GetComponent<NetworkAnimator>();
        _netAnimator.animator = GetComponent<Animator>();

       // thePlayer = GameObject.FindWithTag("Player");
       // if (thePlayer != null)
       //     target = thePlayer.transform;

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

        //var nameTag = Instantiate(enemyText);
        //nameTag.GetComponentInChildren<Transform>().transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        //nameTag.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<EnemyStats>().enemyName.ToString();
        //nameTag.transform.SetParent(transform);
    }
    private void Awake()
    {
        InitializeEnemyStart();
        GetComponent<EnemyStats>().InitializeEnemyStats();
    }   

    public bool freezing = false;
    private Vector2 clientPosition;
    private Vector2 serverPosition;
    private void Update()
    {
        if (isServer)
        {
            if (!freezing)
            {

                EnemyFollow();
                if (_netRigidbody2D.target.velocity != Vector2.zero)
                {
                    _netAnimator.animator.SetFloat("moveX", _netRigidbody2D.target.velocity.x);
                    _netAnimator.animator.SetFloat("moveY", _netRigidbody2D.target.velocity.y);
                    _netAnimator.animator.SetBool("moving", true);
    
                }
                else
                {
                    _netAnimator.animator.SetBool("moving", false);
                }
                //serverPosition = this.transform.position;
                //Debug.Log(serverPosition + "server");
            }
        }
        //if(isClient)
        //{
        //    if (!freezing)
        //    {
        //
        //        EnemyFollow();
        //        if (_netRigidbody2D.target.velocity != Vector2.zero)
        //        {
        //            _netAnimator.animator.SetFloat("moveX", _netRigidbody2D.target.velocity.x);
        //            _netAnimator.animator.SetFloat("moveY", _netRigidbody2D.target.velocity.y);
        //            _netAnimator.animator.SetBool("moving", true);
        //
        //        }
        //        else
        //        {
        //            _netAnimator.animator.SetBool("moving", false);
        //        }
        //        clientPosition = this.transform.position;
        //        //Debug.Log(clientPosition + "client");
        //    }
        //}
        //if (isServer)
        //{
        //    if (clientPosition != serverPosition)
        //        clientPosition = serverPosition;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                return;

            target = collision.gameObject.transform;
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
    [Server]
    void EnemyFollow()
    {
        if (target == null)
            FreeRoam();
        //else
        //{
        //    if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        //    }
        //    if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        //    {
        //        FreeRoam();
        //    }
        //}
    }

    [Command(requiresAuthority = false)]
    public void CmdEnemyMoving(Vector3 _moveDirection,Vector2 newPos)
    {
        _netRigidbody2D.target.velocity = _moveDirection;
        this.transform.position = newPos;
        RpcEnemyMoving(_moveDirection,newPos);
    }
    [ClientRpc]
    public void RpcEnemyMoving(Vector3 _moveDirection,Vector2 newPos)
    {
        this.transform.position = newPos;
        _netRigidbody2D.target.velocity = _moveDirection;
    
    }
    [Command(requiresAuthority = false)]
    public void CmdEnemyNotMoving()
    {
        _netRigidbody2D.target.velocity = Vector2.zero;
        RpcEnemyNotMoving();
    }
    [ClientRpc]
    public void RpcEnemyNotMoving()
    {
        _netRigidbody2D.target.velocity = Vector2.zero;
    }
    [Server]
    void FreeRoam()
    {
        if (isServerOnly)
        {
            if (moving)
            {
                if (isClient)
                    return;
                timeToMoveCounter -= Time.deltaTime;
                _netRigidbody2D.target.velocity = moveDirection;
                if (timeToMoveCounter < 0f)
                {
                    moving = false;
                    timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                }

            }
            else
            {
                if (isClient)
                    return;
                Debug.Log("move into new direction");
                timeBetweenMoveCounter -= Time.deltaTime;
                _netRigidbody2D.target.velocity = Vector2.zero;
                if (timeBetweenMoveCounter < 0f)
                {
                    moving = true;
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                    moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
                }

            }
        }
    }


    [Command(requiresAuthority = false)]
    public void CmdMoving()
    {
        timeToMoveCounter -= Time.deltaTime;
        _netRigidbody2D.target.velocity = moveDirection;
        serverPosition = this.transform.position;
        if (timeToMoveCounter < 0f)
        {
            moving = false;
            timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            RpcMoving(timeBetweenMoveCounter,serverPosition);
        }
    }
    [ClientRpc]
    public void RpcMoving(float timeBetweenCounter,Vector2 serverPos)
    {
        timeToMoveCounter -= Time.deltaTime;
        _netRigidbody2D.target.velocity = moveDirection;
        this.transform.position = serverPos;
        clientPosition = this.transform.position;
        if (timeToMoveCounter < 0f)
        {
            moving = false;
            timeBetweenMoveCounter = timeBetweenCounter;
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdNotMoving()
    {
        timeBetweenMoveCounter -= Time.deltaTime;
        _netRigidbody2D.target.velocity = Vector2.zero;
        if (timeBetweenMoveCounter < 0f)
        {
            moving = true;
            timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
            moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            RpcNotMoving(moveDirection,timeToMoveCounter);
        }
    }
    [ClientRpc]
    public void RpcNotMoving(Vector3 newDirection, float newMoveCounter)
    {
        timeBetweenMoveCounter -= Time.deltaTime;
        _netRigidbody2D.target.velocity = Vector2.zero;
        if (timeBetweenMoveCounter < 0f)
        {
            moving = true;
            timeToMoveCounter = newMoveCounter;
            moveDirection = newDirection;
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
