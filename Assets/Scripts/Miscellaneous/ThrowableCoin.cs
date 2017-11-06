using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableCoin : MonoBehaviour {

	public CircleCollider2D circleCollider;

	public void Throw()
	{
		circleCollider.enabled = true;
		GameController.instance.Spend (GameController.instance.throwableCoinCost);
		StartCoroutine (SelfDestroy ());
	}

	public IEnumerator SelfDestroy()
	{
		yield return new WaitForSeconds (0.2f);
		Destroy (gameObject);
	}
}
