using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveSpeed;

	private void OnEnable()
	{
		Invoke("Destroy", 3);
	}

	void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir)
	{
        moveDirection = dir;
	}

    private void Destroy()
	{
        gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		CancelInvoke();
	}
}
