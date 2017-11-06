using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public int hitsToDie = 5;
	public float speed = 3f;
	public float timeStopedWhenSatisfied = 1f;
	public Animator animatorController;
	public CircleCollider2D mouthCollider;
	public Boundary movementBoundary;

	private int hits;
	private bool canMove;
	private WaitForSeconds satisfiedDelay;
	private Vector3 target;
	private IEnumerator movingRoutine;

	void Start()
	{
		canMove = true;
		hits = 0;
		satisfiedDelay = new WaitForSeconds (timeStopedWhenSatisfied);
	}

	void Update()
	{
		if (canMove) 
		{
			target.x = Random.Range (movementBoundary.xMin, movementBoundary.xMax);
			target.y = Random.Range (movementBoundary.yMin, movementBoundary.yMax);
			target.z = target.y + 0.5f;

			movingRoutine = Move ();

			StartCoroutine (movingRoutine);
		}

	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if(other.tag == "Throwable")
		{
			Hit ();
			Destroy (other.gameObject);
		}
		else if(other.tag == "Building")
		{
			StartCoroutine (Satisfied ());
			Destroy (other.gameObject);
		}
	}

	public void SetMouthToTrue()
	{
		mouthCollider.enabled = true;
	}

	public void SetMouthToFalse()
	{
		mouthCollider.enabled = false;
	}

	public void Hit()
	{
		hits++;
		StartCoroutine (Satisfied ());
	}

	private IEnumerator Move()
	{
		canMove = false;

		float step;
		while((transform.position - target).magnitude > float.Epsilon)
		{
			step = Time.deltaTime * speed;
			transform.position = Vector3.MoveTowards (transform.position, target, step);
			yield return null;
		}

		canMove = true;
	}

	private IEnumerator Satisfied()
	{
		StopCoroutine (movingRoutine);
		animatorController.SetTrigger ("Satisfied");
		canMove = false;

		yield return satisfiedDelay;

		if (!Died ()) 
		{
			canMove = true;
			animatorController.SetTrigger ("Normal");
		}
		else
		{
			Die ();
		}

	}

	private bool Died()
	{
		return hits >= hitsToDie;
	}

	private void Die()
	{
		GameController.instance.DefeatMonster ();
		Destroy (gameObject);
	}
}
