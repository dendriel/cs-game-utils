using UnityEngine;


/// <summary>
/// Add this component to any GO that must be follow by the camera.
/// 
/// It will register himself in the Camera2DFollowMany.
/// </summary>
public class CameraFollowElement : MonoBehaviour
{
	Camera2DFollowMany cameraFollow;

	void OnEnable()
	{
		cameraFollow = Camera.main.GetComponent<Camera2DFollowMany>();
		cameraFollow.AddPlayer(this.gameObject);
	}

	void OnDisable()
	{
		// Won't be followed if disabled.
		cameraFollow.RemovePlayer(this.gameObject);
	}
}
