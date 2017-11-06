using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public CircleCollider2D mouthCollider;

	public void SetMouthToTrue()
	{
		mouthCollider.enabled = true;
	}

	public void SetMouthToFalse()
	{
		mouthCollider.enabled = false;
	}
}
