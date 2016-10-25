using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.
There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/

	// The target we are following
	public Transform target;
	//the offset from the target point
	public Vector3 targetOffset = new Vector3 (0, 2f, 0);
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;

	// How much we
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	void OnEnable ()
	{
        //EventManager.Instance.StartListeningOnce<ChunkEnteredEvent>(MoveWithPlayer);
    }

	void OnDisable ()
	{
        //    EventManager.Instance.StopListening<ChunkEnteredEvent>(MoveWithPlayer);
    }

	void Start() {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;

		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

	        // Set the position of the camera on the x-z plane to:
	        // distance meters behind the target
	        var newPos = target.position;
	        newPos -= currentRotation*Vector3.forward*distance;

	        newPos = new Vector3(newPos.x, currentHeight, newPos.z);

	        // Set the height of the camera
	        transform.position = Vector3.MoveTowards(transform.position, newPos, 0.5f);

        // Always look at the target
        transform.LookAt (target.position + targetOffset);
	}
}