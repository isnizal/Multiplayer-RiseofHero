using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCMovement : MonoBehaviour
{
    private enum NpcCondition {Idle, Walk, Talking};
    private NpcCondition currentConditionNPC;
    private Rigidbody2D rb2D;
    private bool isTalking = false;
    private Animator anim;

    [FormerlySerializedAs("movePointsRight")]
    public Transform[] movePointsPos;
    public int randomPosRight;
    public int randomPosUp;
    public float moveSpeed;
    public bool isIdle = false;
    public int whereMove;
    public bool moveRight;
    public bool moveLeft;
    public bool moveUp;
    public bool moveDown;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
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
        if (currentConditionNPC == NpcCondition.Walk)
        {
            if (!isTalking && !isIdle)
            {
                if (whereMove == 0)
                {

                    if (transform.position.x != movePointsPos[randomPosRight].transform.position.x)
                    {
                        // Debug.Log(movePointsRight[randomPosRight].transform.position.x);
                        if (moveRight)
                        {
                            rb2D.velocity = new Vector2(moveSpeed, 0);
                        }
                        else if (moveLeft)
                        {
 
                            rb2D.velocity = new Vector2(-moveSpeed, 0);
                        }
                    }
                   // else
                   // {
                   //     rigidbody2D.velocity = Vector2.zero;
                   // }
                }
                else if (whereMove == 1)
                {
                    if (transform.position.y != movePointsPos[randomPosUp].transform.position.y)
                    {
                        if (moveUp)
                        {
                            rb2D.velocity = new Vector2(0, moveSpeed);
                        }
                        else if (moveDown)
                        {
                            rb2D.velocity = new Vector2(0, -moveSpeed);
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
        }
        else if (currentConditionNPC == NpcCondition.Idle)
        {
            isIdle = true;
            
        }
        AnimationUpdate();
        InitializeMove();
    }
    private void AnimationUpdate()
    {
        if (moveRight)
        {
            if (isIdle)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", true);

                anim.SetFloat("IdleX", 1);
                anim.SetFloat("IdleY", 0);

                anim.SetFloat("TalkX", 0);
                anim.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", true);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", false);

                anim.SetFloat("IdleX", 0);
                anim.SetFloat("IdleY", 0);

                anim.SetFloat("TalkX", 1);
                anim.SetFloat("TalkY", 0);
            }

        }
        else if (moveLeft)
        {
           
            if (isIdle)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", true);

                anim.SetFloat("IdleX", -1);
                anim.SetFloat("IdleY", 0);

                anim.SetFloat("TalkX", 0);
                anim.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", true);

                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", false);

                anim.SetFloat("IdleX", 0);
                anim.SetFloat("IdleY", 0);

                anim.SetFloat("TalkX", -1);
                anim.SetFloat("TalkY", 0);
            }
        }
        else if (moveUp)
        {
            
            if (isIdle)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", true);

                anim.SetFloat("IdleX", 0);
                anim.SetFloat("IdleY", 1);

                anim.SetFloat("TalkX", 0);
                anim.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", true);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", false);

                anim.SetFloat("IdleX", 0);
                anim.SetFloat("IdleY", 0);

                anim.SetFloat("TalkX", 0);
                anim.SetFloat("TalkY", 1);
            }
        }
        else if (moveDown)
        {
           
            if (isIdle)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", true);

                anim.SetFloat("IdleX", 0);
                anim.SetFloat("IdleY", -1);

                anim.SetFloat("TalkX", 0);
                anim.SetFloat("TalkY", 0);
            }
            else if (isTalking)
            {
                rb2D.velocity = Vector2.zero;
                anim.SetBool("isTalk", true);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", false);

                anim.SetFloat("IdleX", 0);
                anim.SetFloat("IdleY", 0);

                anim.SetFloat("TalkX", 0);
                anim.SetFloat("TalkY", -1);
            }
        }

    }
    private void InitializeMove()
    {
        anim.SetFloat("moveX", rb2D.velocity.x);
        anim.SetFloat("moveY", rb2D.velocity.y);
        if (rb2D.velocity != Vector2.zero)
        {
            anim.SetBool("isWalk", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isTalk", false);
        }
    }
    private NpcCondition lastCurrentCondition;
    public void ActivateTalkCondition()
    {
        lastCurrentCondition = currentConditionNPC;
        currentConditionNPC = NpcCondition.Talking;
    }

    public void DeactivateTalkCondition()
    {
        isTalking = false;
        isIdle = false;
        currentConditionNPC = lastCurrentCondition;
    }

    private IEnumerator Waiting;
    public int idleChance = 0;
    public void SetRandomCondition()
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

    public IEnumerator WaitingIdle(float value)
    {
        yield return new WaitForSeconds(value);
        currentConditionNPC = NpcCondition.Walk;
        isIdle = false;
    }
}
