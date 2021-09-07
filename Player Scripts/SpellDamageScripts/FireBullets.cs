using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [SerializeField] int bulletsAmount = 10;

    [SerializeField] float startAngle = 90f, endAngle = 270f;

    private Vector2 bulletMoveDirection;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
		{
            Fire();
		}
    }

    private void Fire()
	{
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

		for (int i = 0; i < bulletsAmount + 1; i++)
		{
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<Bullet>().SetMoveDirection(bulDir);
            angle += angleStep;
		}
	}
}
