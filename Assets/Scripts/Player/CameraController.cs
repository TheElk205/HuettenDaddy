using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public Transform followMe;
    private float initialCameraZ = 0.0f;
    
    public Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = followMe.position - camera.transform.position;
        initialCameraZ = camera.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = followMe.position + cameraOffset;
        newPosition.z = initialCameraZ;

        camera.transform.position = newPosition;
    }
}
