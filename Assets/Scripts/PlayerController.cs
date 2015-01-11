using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	//public float speed;
	void Update ()
	{
		//Write an if statement here - if on a tablet use accel
		// if on computer use getAxis.

		//float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveVertical = Input.GetAxis ("Vertical");

		float moveHorizontal = Input.acceleration.x;
		float moveVertical = Input.acceleration.y;

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigidbody.AddForce(movement * 50);
		//rigidbody.AddForce(movement * speed * Time.deltaTime);
	}
}

