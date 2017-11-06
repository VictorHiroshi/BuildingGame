using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class MonsterController : MonoBehaviour {

	public float speed = 3f;
	public AnimatorController animatorController;
	public CircleCollider2D mouthCollider;
	public Boundary movementBoundary;

	private bool moving;
	private Vector3 target;

	void Start()
	{
		moving = false;
	}

	void Update()
	{
		if (moving)
			return;

		target.x = Random.Range (movementBoundary.xMin, movementBoundary.xMax);
		target.y = Random.Range (movementBoundary.yMin, movementBoundary.yMax);

		StartCoroutine (Move ());

	}

	public void SetMouthToTrue()
	{
		mouthCollider.enabled = true;
	}

	public void SetMouthToFalse()
	{
		mouthCollider.enabled = false;
	}

	private IEnumerator Move()
	{
		moving = true;

		float step;
		while((transform.position - target).magnitude > float.Epsilon)
		{
			step = Time.deltaTime * speed;
			transform.position = Vector3.MoveTowards (transform.position, target, step);
			yield return null;
		}

		moving = false;
	}
}
