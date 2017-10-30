using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCoinWallet : MonoBehaviour {

	private Vector3 target;

	void LateUpdate () {
		
		target = GameController.instance.GetCoinPosition ();

		transform.position = Vector3.MoveTowards (transform.position, target, GameController.instance.coinSpeed);

		if ((transform.position - target).magnitude < float.Epsilon)
		{
			// TODO: Play coin sound.

			GameController.instance.Receive (1);

			Destroy (gameObject);
		}
	}
}
