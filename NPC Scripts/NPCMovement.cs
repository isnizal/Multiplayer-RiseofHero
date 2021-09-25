using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Mirror;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(Mirror.Experimental.NetworkRigidbody2D))]
public class NPCMovement : NetworkBehaviour
{
    private enum NpcCondition {Idle, Walk, Talking};
    [SyncVar]
    private NpcCondition currentConditionNPC;
    //private Rigidbody2D rb2D;
    private Mirror.Experimental.NetworkRigidbody2D _netrb2D;
    private NetworkTransform _netTransform;
    private NetworkAnimator _netAnimator;
    [SyncVar]
    private bool isTalking = false;
    //private Animator anim;

    [FormerlySerializedAs("movePointsRight")]
    public Transform[] movePointsPos;
    public int randomPosRight;
    public int randomPosUp;
    public float moveSpeed;
    
    [SyncVar]public bool isIdle = false;


    public int whereMove;
    public bool moveRight;
    public bool moveLeft;
    public bool moveUp;
    public bool moveDown;

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {

            //anim = GetComponent<Animator>();
            //rb2D = GetComponent<Rigidbody2D>();


        }
        //if (isClient)
        //{
        //    //anim = GetComponent<Animator>();
        //    //rb2D = GetComponent<Rigidbody2D>();
        //    currentConditionNPC = NpcCondition.Walk;
        //    randomPosRight = Random.Range(0, movePointsPos.Length);
        //    whereMove = 0;
        //    isTalking = false;
        //    isIdle = false;
        //    moveRight = true;
        //    movePointsPos = new Transform[5];
        //    _patrolPos = GameObject.Find("PatrolPos(Clone)");
        //    for (int i = 0; i < movePointsPos.Length; i++)
        //    {
        //        movePointsPos[i] = _patrolPos.gameObject.transform.GetChild(i).transform;
        //    }
        //}

    }
    private GameObject _patrolPos;
    [Server]
    public void AssignMovePointPos(GameObject _patrolPos)
    {
        movePointsPos = new Transform[5];
        for (int i = 0; i < movePointsPos.Length; i++)
        {
            movePointsPos[i] = _patrolPos.gameObject.transform.GetChild(i).transform;
        }
        _netrb2D = GetComponent<Mirror.Experimental.NetworkRigidbody2D>();
        _netTransform = GetComponent<NetworkTransform>();
        _netAnimator = GetComponent<NetworkAnimator>();
        currentConditionNPC = NpcCondition.Walk;
        randomPosRight = Random.Range(0, movePointsPos.Length);
        whereMove = 0;
        isTalking = false;
        isIdle = false;
        moveRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (currentConditionNPC == NpcCondition.Walk)
            {
                if (!isTalking && !isIdle)
                {
                    if (whereMove == 0)
                    {
                        _netTransform = GetComponent<NetworkTransform>();
                        if (_netTransform.transform.position.x != movePointsPos[randomPosRight].transform.position.x)
                        {
                            // Debug.Log(movePointsRight[randomPosRight].transform.position.x);
                            if (moveRight)
                            {
                                _netrb2D.target.velocity = new Vector2(moveSpeed, 0);
                            }
                            else if (moveLeft)
                            {

                                _netrb2D.target.velocity = new Vector2(-moveSpeed, 0);
                            }
                        }
                        // else
                        // {
                        //     rigidbody2D.velocity = Vector2.zero;
                        // }
                    }
                    else if (whereMove == 1)
                    {
                        _netTransform = GetComponent<NetworkTransform>();
                        if (_netTransform.transform.position.y != movePointsPos[randomPosUp].transform.position.y)
                        {
                            if (moveUp)
                            {
                                _netrb2D.target.velocity = new Vector2(0, moveSpeed);
                            }
                            else if (moveDown)
                            {
                                _netrb2D.target.velocity = new Vector2(0, -moveSpeed);
                            }
                        }
                        // else
                        // {
                        //     rigidbody2D.velocity = Vector2.zero;
                        // }
                    }
                }
            }
            else if (currentConditionNPC == NpcCondition.Talking)
            {
                isTalking = true;
                StartCoroutine(SetToWalk());
            }
            else if (currentConditionNPC == NpcCondition.Idle)
            {
                isIdle = true;
                StartCoroutine(SetToWalk());
            }
            AnimationUpdate();
            InitializeMove();
        }
    }
    private IEnumerator SetToWalk()
    {
        yield return new WaitForSeconds(15f);
        isTalking = false;
        isIdle = false;
        currentConditionNPC = NpcCondition.Walk;
    }
    [Server]
    private void AnimationUpdate()
    {
        if (moveRight)
        {
            if (isIdle)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", false);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", true);

                _netAnimator.animator.SetFloat("IdleX", 1);
                _netAnimator.animator.SetFloat("IdleY", 0);

                _netAnimator.animator.SetFloat("TalkX", 0);
                _netAnimator.animator.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", true);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", false);

                _netAnimator.animator.SetFloat("IdleX", 0);
                _netAnimator.animator.SetFloat("IdleY", 0);

                _netAnimator.animator.SetFloat("TalkX", 1);
                _netAnimator.animator.SetFloat("TalkY", 0);
            }

        }
        else if (moveLeft)
        {
           
            if (isIdle)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", false);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", true);

                _netAnimator.animator.SetFloat("IdleX", -1);
                _netAnimator.animator.SetFloat("IdleY", 0);

                _netAnimator.animator.SetFloat("TalkX", 0);
                _netAnimator.animator.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", true);

                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", false);

                _netAnimator.animator.SetFloat("IdleX", 0);
                _netAnimator.animator.SetFloat("IdleY", 0);

                _netAnimator.animator.SetFloat("TalkX", -1);
                _netAnimator.animator.SetFloat("TalkY", 0);
            }
        }
        else if (moveUp)
        {
            
            if (isIdle)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", false);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", true);

                _netAnimator.animator.SetFloat("IdleX", 0);
                _netAnimator.animator.SetFloat("IdleY", 1);

                _netAnimator.animator.SetFloat("TalkX", 0);
                _netAnimator.animator.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", true);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", false);

                _netAnimator.animator.SetFloat("IdleX", 0);
                _netAnimator.animator.SetFloat("IdleY", 0);

                _netAnimator.animator.SetFloat("TalkX", 0);
                _netAnimator.animator.SetFloat("TalkY", 1);
            }
        }
        else if (moveDown)
        {
           
            if (isIdle)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", false);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", true);

                _netAnimator.animator.SetFloat("IdleX", 0);
                _netAnimator.animator.SetFloat("IdleY", -1);

                _netAnimator.animator.SetFloat("TalkX", 0);
                _netAnimator.animator.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                _netrb2D.target.velocity = Vector2.zero;
                _netAnimator.animator.SetBool("isTalk", true);
                _netAnimator.animator.SetBool("isWalk", false);
                _netAnimator.animator.SetBool("isIdle", false);

                _netAnimator.animator.SetFloat("IdleX", 0);
                _netAnimator.animator.SetFloat("IdleY", 0);

                _netAnimator.animator.SetFloat("TalkX", 0);
                _netAnimator.animator.SetFloat("TalkY", -1);
            }
        }

    }
    [Server]
    private void InitializeMove()
    {
        _netAnimator.animator.SetFloat("moveX", _netrb2D.target.velocity.x);
        _netAnimator.animator.SetFloat("moveY", _netrb2D.target.velocity.y);
        if (_netrb2D.target.velocity != Vector2.zero)
        {
            _netAnimator.animator.SetBool("isWalk", true);
            _netAnimator.animator.SetBool("isIdle", false);
            _netAnimator.animator.SetBool("isTalk", false);
        }
    }
    [SyncVar]
    private NpcCondition lastCurrentCondition;
    [Client]
    public void ActivateTalkCondition()
    {
        CmdActivateTalkCondition();
    }
    [Command(requiresAuthority = false)]
    public void CmdActivateTalkCondition()
    {
        lastCurrentCondition = currentConditionNPC;
        currentConditionNPC = NpcCondition.Talking;
    }
    [Client]
    public void DeactivateTalkCondition()
    {
        CmdDeactivateTalkCondition();
    }
    [Command(requiresAuthority = false)]
    public void CmdDeactivateTalkCondition()
    {
        isTalking = false;
        isIdle = false;
        currentConditionNPC = lastCurrentCondition;
    }
    public IEnumerator WaitingIdle(float value)
    {
        yield return new WaitForSeconds(value);
        currentConditionNPC = NpcCondition.Walk;
        isIdle = false;
    }
    private IEnumerator Waiting;
    public int idleChance = 0;
    //server
    public void SetRandomCondition()
    {
        CmdSetRandomCondition();
    }
    public void CmdSetRandomCondition()
    {
        int randomCondition = Random.Range(0, 2);
        if (randomCondition == 0)
        {
            idleChance--;
            if (!isTalking)
            {
                currentConditionNPC = NpcCondition.Walk;
            }
        }
        else if (randomCondition == 1)
        {
            idleChance++;
            if (idleChance == 3)
            {
                if (!isTalking)
                {
                    currentConditionNPC = NpcCondition.Idle;
                    Waiting = WaitingIdle(5f);
                    StartCoroutine(Waiting);
                }
            }
        }
    }
    public void SetRandomPosRight(HitPatrolPos.PatPosType pos)
    {
        CmdSetRandomPosRight(pos);
    }
    public void CmdSetRandomPosRight(HitPatrolPos.PatPosType pos)
    {
        if (pos == HitPatrolPos.PatPosType.PatLeft)
        {
            whereMove = 0;
            randomPosRight = Random.Range(1, movePointsPos.Length);
            moveRight = true;
            moveLeft = false;
            if (whereMove == 0)
            {
                moveUp = false;
                moveDown = false;
            }
        }
        else if (pos == HitPatrolPos.PatPosType.PatMiddle)
        {
            randomPosRight = Random.Range(0, 2);
            if (randomPosRight == 0)
            {
                randomPosRight = 0;
                moveLeft = true;
                moveRight = false;
            }
            else if (randomPosRight == 1)
            {
                randomPosRight = 4;
                moveRight = true;
                moveLeft = false;
            }
            if (whereMove == 0)
            {
                moveUp = false;
                moveDown = false;
            }
        }
        else if (pos == HitPatrolPos.PatPosType.PatRight)
        {
            randomPosRight = Random.Range(0, 2);
            moveRight = false;
            moveLeft = true;
            if (whereMove == 0)
            {
                moveUp = false;
                moveDown = false;
            }
        }
    }
    public void SetRandomPosUp(HitPatrolPos.PatPosType posUp,HitPatrolPos.PatPosType posRight)
    {
        CmdSetRandomPosUp(posUp, posRight);
    }
    public void CmdSetRandomPosUp(HitPatrolPos.PatPosType posUp, HitPatrolPos.PatPosType posRight)
    {
        if (whereMove == 1)
        {
            moveLeft = false;
            moveRight = false;
        }
        if (posUp == HitPatrolPos.PatPosType.PatBelow)
        {
            moveDown = true;
            moveUp = false;
        }
        else if (posUp == HitPatrolPos.PatPosType.PatUp)
        {
            moveUp = true;
            moveDown = false;
        }
    }


}
