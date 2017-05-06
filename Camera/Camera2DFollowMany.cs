using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Camera 2D Follow Many will follow any CameraFollowElement (component).
///
/// If there is multiple GOs to follow, the camera will expand accordingly to
/// the configured parameters.
/// </summary>
public class Camera2DFollowMany : MonoBehaviour
{

	[Tooltip("Camera's speed while following")]
	public float FollowSpeed = 0.1f;
	
	[Tooltip("Add a vertical offset for the camera")]
	public float VerticalOffset = 0f;
	[Tooltip("Add a horizontal offset for the camera")]
	public float HorizontalOffset = 0f;

	[Tooltip("Maximum expanded size of the camera.")]
	public float MaximumCameraSize = 10f;

	[Tooltip("Expand the camera horizontally")]
	public bool ExpandCameraX = true;
	public float MinDistanceXToExpand = 10f;

	[Tooltip("Expand the camera vertically")]
	public bool ExpandCameraY = true;
	public float MinistanceYToExpand = 6f;
	

    [Header("The camera won't go further these bounds")]
    public Transform TopLimit;
    public Transform BottomLimit;
    public Transform LeftLimit;
    public Transform RightLimit;

    private List<GameObject> targetList;
	private Camera cameraRef;
	private float defaultCameraSize;

	void Awake()
    {
        if (targetList == null) targetList = new List<GameObject>();

		cameraRef = GetComponent<Camera> ();
		defaultCameraSize = cameraRef.orthographicSize;
	}

	void Update()
	{
		if (targetList.Count == 0) {
			return;
		}
		
		Vector3 posHigher = new Vector3 (FindHigherX (), FindHigherY ());
		Vector3 posLower = new Vector3 (FindLowerX (), FindLowerY ());

		UpdateCameraPosition (posHigher, posLower);
		UpdateCameraSize(posHigher, posLower);
	}

	/// <summary>
	/// Add an element to be followed.
	/// </summary>
	/// <param name="element">The element to be followed.</param>
	public void AddPlayer(GameObject element)
	{
        if (targetList == null) targetList = new List<GameObject>();
        targetList.Add (element);
	}

	/// <summary>
	/// Unfollow an element.
	/// </summary>
	/// <param name="element">The element to stop following.</param>
	public void RemovePlayer(GameObject element)
	{
		if (targetList == null) return; // One may want to throw an exception.

		targetList.Remove (element);
	}

	void UpdateCameraPosition(Vector3 posHigher, Vector3 posLower)
	{
		Vector3 middle = (posHigher + posLower) * 0.5f;

		// Update the camera position.
		Vector3 targetPos = new Vector3( middle.x + HorizontalOffset, middle.y + VerticalOffset, cameraRef.transform.position.z );

        // Fix targetPos to the limit.
        float halfVerticalSize = Camera.main.orthographicSize;
        float halfHorizontalSize = halfVerticalSize * Screen.width / Screen.height;

        if (TopLimit != null) {
            float topLimit = TopLimit.transform.position.y - halfVerticalSize;
            targetPos.y = Mathf.Min(topLimit, targetPos.y);
        }
        if (BottomLimit != null) {
            float bottomLimit = BottomLimit.transform.position.y + halfVerticalSize;
            targetPos.y = Mathf.Max(bottomLimit, targetPos.y);
        }
        if (LeftLimit != null) {
            float leftLimit = LeftLimit.transform.position.x + halfHorizontalSize;
            targetPos.x = Mathf.Max(leftLimit, targetPos.x);
        }
        if (RightLimit != null) {
            float rightLimit = RightLimit.transform.position.x - halfHorizontalSize;
            targetPos.x = Mathf.Min(rightLimit, targetPos.x);
        }

        float journeyLength = Vector3.Distance(transform.position, targetPos);
		float distanceCovered = FollowSpeed; // speed
		float fracJourney = distanceCovered / journeyLength;

		cameraRef.transform.position = Vector3.Lerp(transform.position, targetPos, fracJourney);
	}
	
	/// <summary>
	/// Expand the camera size if distance between game objects is higher than the minimum range to expand.
	/// and focus the camera to the default size if the game objects are inside the minimum range.
	/// 
	/// Expand in a rate of half the difference between "dist out of the minimum range" and the minimum range.
	/// </summary>
	/// <param name="posHigher"></param>
	/// <param name="posLower"></param>
	void UpdateCameraSize(Vector3 posHigher, Vector3 posLower)
	{
		Vector3 distance = posHigher - posLower;
		float expandRate = 0;
		
		if (ExpandCameraX) {
			expandRate = Mathf.Max (0, (distance.x - MinDistanceXToExpand)) / 2;
		}
		
		if (ExpandCameraY) {
			expandRate += Mathf.Max (0, (distance.y - MinistanceYToExpand)) / 2;
		}
		
		// Update camera size.
		if (MaximumCameraSize > 0) {
			cameraRef.orthographicSize = Mathf.Min (MaximumCameraSize, (defaultCameraSize + expandRate));
		}
		else {
			cameraRef.orthographicSize = defaultCameraSize + expandRate;
		}
	}

	private float FindHigherX()
	{
		float x = targetList[0].GetComponent<Transform>().position.x;

		foreach (GameObject pos in targetList) {
			if (pos.GetComponent<Transform>().position.x > x) {
				x = pos.GetComponent<Transform>().position.x;
			}
		}

		return x;
	}
	
	private float FindHigherY()
	{
		float y = targetList[0].GetComponent<Transform>().position.y;
		
		foreach (GameObject pos in targetList) {
			if (pos.GetComponent<Transform>().position.y > y) {
				y = pos.GetComponent<Transform>().position.y;
			}
		}
		
		return y;
	}
	
	private float FindLowerX()
	{
		float x = targetList[0].GetComponent<Transform>().position.x;
		
		foreach (GameObject pos in targetList) {
			if (pos.GetComponent<Transform>().position.x < x) {
				x = pos.GetComponent<Transform>().position.x;
			}
		}
		
		return x;
	}
	
	private float FindLowerY()
	{
		float y = targetList[0].GetComponent<Transform>().position.y;
		
		foreach (GameObject pos in targetList) {
			if (pos.GetComponent<Transform>().position.y < y) {
				y = pos.GetComponent<Transform>().position.y;
			}
		}
		
		return y;
	}
}
