using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
	public static BossAI instance;
	public static BossAI BossInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<BossAI>();
			}
			return instance;
		}

	}

	[Header("----> Boss Movement <----")]
    public int speed;
    public Transform[] moveSpots;
    private int randomSpot;
	private float waitTime;
	public float startWaitTime;
	private Transform[] wayPoints = new Transform[5];

	private void Start()
	{
		waitTime = startWaitTime;
		randomSpot = Random.Range(0, wayPoints.Length);

		for (int i = 0; i < wayPoints.Length; i++)
		{
			wayPoints[i] = GameObject.Find("Dungeons and Caves").transform.GetChild(0).transform.GetChild(i);
		}


	}

	private void Update()
	{

		transform.position = Vector2.MoveTowards(transform.position, wayPoints[randomSpot].position, speed * Time.deltaTime);

		if (Vector2.Distance(transform.position, wayPoints[randomSpot].position) < 0.2f)
		{
			if (waitTime <= 0)
			{
				randomSpot = Random.Range(0, wayPoints.Length);
				waitTime = startWaitTime;
			}
			else
			{ waitTime -= Time.deltaTime; }
		}
	}
}
