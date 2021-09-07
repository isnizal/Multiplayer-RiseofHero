using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
	[SerializeField] private float scanRadius = 3f;
	[SerializeField] private LayerMask layers;
	[SerializeField] private Collider2D target;
	[Space]
	[SerializeField] private GameObject turretBullet;
	[SerializeField] private Transform firePoint;
	[SerializeField] private float fireRate = 1f;


	private void Start()
	{
		InvokeRepeating(nameof(Fire), 0f, fireRate);
	}

	private void Update()
	{
		CheckEnviroment();
		LookAtTarget();
	}

	private void CheckEnviroment()
	{
		target = Physics2D.OverlapCircle(transform.position, scanRadius, layers);
	}

	private void LookAtTarget()
	{
		if(target != null)
		{
			Vector2 direction = target.transform.position - transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	private void Fire()
	{
		if(target != null)
		{
			Instantiate(turretBullet, firePoint.position, firePoint.rotation);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, scanRadius);
	}
}
