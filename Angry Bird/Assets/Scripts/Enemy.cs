using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public GameObject deathEffect;

	public float health = 4f;

    private void Start()
    {
        GameManager.EnemyAlive++;
    }

    private void OnCollisionEnter2D (Collision2D colInfo)
	{
		if (colInfo.collider.tag == "Bird" || colInfo.relativeVelocity.magnitude > health)
		{
			Die();
		}
	}

	void Die ()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);

        GameManager.EnemyAlive--;

        if (GameManager.EnemyAlive <= 0)
            GameManager.instance.Win();

        Destroy(gameObject);
	}

}
