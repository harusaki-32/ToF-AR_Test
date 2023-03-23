using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectPlacement : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject SpawnObjectPrefab;
    public ARRaycastManager arRaycastManager;

    public void Update()
    {
        Touch touch;
        if (Input.touchCount > 0 && (touch = Input.GetTouch(0)).phase == TouchPhase.Began)
        {
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

            if (arRaycastManager.Raycast(touch.position, hitResults, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitResults)
                {
                    if (Vector3.Dot(MainCamera.transform.position - hit.pose.position, hit.pose.up) > 0)
                    {
                        // Instantiate a new game object on the hit plane
                        Vector3 position = hit.pose.position;
                        position.y += 0.15f;
                        var planeObject = Instantiate(SpawnObjectPrefab, position, hit.pose.rotation);
                    }
                }
            }
        }
    }
}