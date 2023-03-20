using UnityEngine;

public class CameraController : MonoBehaviour
{
    // rotation speed for horizontal
    public float rotationSpeedX = 2f;
    // rotation speed for vertical
    public float rotationSpeedY = 2f;
    // speed for moving camera position
    public float movementSpeed = 10f;

    // target for setting the inital position of the camera
    public Transform target;

    private float _yaw = 0f;
    private float _pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 offset = new Vector3(0f, 5f, 0f); // 5 unit above the target
        transform.position = target.position + offset;
        transform.rotation = Quaternion.LookRotation(Vector3.down); // look down along the negative y-axis
    }

    // Update is called once per frame
    void Update()
    {
        // WSAD key movement
        float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        float horizontalInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);

        /* cursor */
        // get the mouse position and multiply by the speed
        _yaw += Input.GetAxis("Mouse X") * rotationSpeedX;
        _pitch -= Input.GetAxis("Mouse Y") * rotationSpeedY;

        // range
        _yaw = Mathf.Clamp(_yaw, -90f, 90f);
        _pitch = Mathf.Clamp(_pitch, -60f, 90f);

        // rotation in world space
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
    }
}
