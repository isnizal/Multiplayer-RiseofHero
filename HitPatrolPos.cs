using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HitPatrolPos : MonoBehaviour
{
    public enum PatPosType {None, PatLeft,PatMiddle,PatRight,PatBelow,PatUp};
    public PatPosType Pos;
    public PatPosType SecondPos;

    private float bxcdBoundX;
    private float bxcdBoundY;
    // Start is called before the first frame update
    void Start()
    {

            bxcdBoundX = GetComponent<BoxCollider2D>().bounds.center.x;
            bxcdBoundY = GetComponent<BoxCollider2D>().bounds.center.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool changeSide;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Patrol")
        {
            NPCMovement npcMovement = collision.gameObject.GetComponent<NPCMovement>();
            npcMovement.SetRandomCondition();
            changeSide = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Patrol")
        {
            changeSide = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Patrol")
        {
            float patrolBoxX = collision.gameObject.GetComponent<BoxCollider2D>().bounds.center.x;
            float patrolBoxY = collision.gameObject.GetComponent<BoxCollider2D>().bounds.center.y;
            NPCMovement npcMovement = collision.gameObject.GetComponent<NPCMovement>();
            if (Pos == PatPosType.PatLeft)
            {
                if (npcMovement.moveLeft)
                {
                    if (patrolBoxX <= bxcdBoundY)
                    {
                        if (changeSide)
                        {
                            changeSide = false;
                            npcMovement.whereMove = 0;
                            npcMovement.SetRandomPosRight(Pos);

                        }
                    }
                }
            }
            else if (Pos == PatPosType.PatMiddle)
            {
                if (npcMovement.moveRight)
                {
                    if (patrolBoxX >= bxcdBoundX)
                    {
                        if (changeSide)
                        {
                            changeSide = false;
                            int randomMove = Random.Range(0, 2);
                            npcMovement.whereMove = randomMove;
                            npcMovement.SetRandomPosRight(Pos);
                            if (SecondPos != PatPosType.None)
                                npcMovement.SetRandomPosUp(SecondPos, Pos);
                        }
                    }
                }
                else if (npcMovement.moveLeft)
                {
                    if (patrolBoxX <= bxcdBoundX)
                    {
                        if (changeSide)
                        {
                            changeSide = false;
                            int randomMove = Random.Range(0, 2);
                            npcMovement.whereMove = randomMove;
                            npcMovement.SetRandomPosRight(Pos);
                            if (SecondPos != PatPosType.None)
                                npcMovement.SetRandomPosUp(SecondPos, Pos);
                        }
                    }
                }
                else if (npcMovement.moveUp)
                {
                    if (patrolBoxY >= bxcdBoundY)
                    {
                        if (changeSide)
                        {
                            changeSide = false;
                            int randomMove = Random.Range(0, 2);
                            npcMovement.whereMove = randomMove;
                            npcMovement.SetRandomPosRight(Pos);
                            if (SecondPos != PatPosType.None)
                                npcMovement.SetRandomPosUp(SecondPos, Pos);
                        }
                    }
                }
            }
            else if (Pos == PatPosType.PatRight)
            {
                if (npcMovement.moveRight)
                {
                    if (patrolBoxX >= bxcdBoundX)
                    {
                        if (changeSide)
                        {
                            changeSide = false;
                            int randomMove = Random.Range(0, 2);
                            npcMovement.whereMove = randomMove;
                            npcMovement.SetRandomPosRight(Pos);
                            if (SecondPos != PatPosType.None)
                                npcMovement.SetRandomPosUp(SecondPos, Pos);
                        }
                    }
                }
                else if (npcMovement.moveUp)
                {
                    if (patrolBoxY >= bxcdBoundY)
                    {
                        if (changeSide)
                        {
                            changeSide = false;
                            int randomMove = Random.Range(0, 2);
                            npcMovement.whereMove = randomMove;
                            npcMovement.SetRandomPosRight(Pos);
                            if (SecondPos != PatPosType.None)
                                npcMovement.SetRandomPosUp(SecondPos, Pos);
                        }
                    }
                }
            }
            else if (Pos == PatPosType.PatUp)
            {
                if (npcMovement.moveDown)
                {
                    if (patrolBoxY <= bxcdBoundY)
                    {
                        if (changeSide)
                        {
                            npcMovement.whereMove = 1;
                            npcMovement.SetRandomPosUp(Pos, PatPosType.None);
                        }
                    }
                }
            }
            //else if (Pos == PatPosType.PatBelow)
            //{
            //    npcMovement.whereMove = 1;
            //    npcMovement.SetRandomPosUp(Pos,PatPosType.None);
            //
            //}
        }
    }
}
