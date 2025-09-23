using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 2.5f;
    [SerializeField] private float _height = 0f;
    [SerializeField] private Vector2 _zoomLimits = new Vector2(25f, 100f);

    private PlayerControls _playerControls;

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Update()
    {
        Move(_playerControls.Camera.Move.ReadValue<Vector2>());
        Zoom(_playerControls.Camera.Zoom.ReadValue<float>());
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Move(Vector2 direction)
    {
        Vector3 translatedDirection = new Vector3(direction.x, 0f, direction.y);
        Vector3 offset = translatedDirection.normalized * _moveSpeed * Time.deltaTime;
        offset.y = _height;
        transform.position += offset;
    }

    private void Zoom(float zoomInput)
    {
        float newFieldOfView = _camera.fieldOfView - zoomInput * _zoomSpeed;
        _camera.fieldOfView = Mathf.Clamp(newFieldOfView, _zoomLimits.x, _zoomLimits.y);
    }
}
