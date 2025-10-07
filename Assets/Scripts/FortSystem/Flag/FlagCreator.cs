using UnityEngine;
using UnityEngine.InputSystem;

public class FlagCreator : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _rayDistance = 1000f;
    [SerializeField] private InputAction _clickAction;
    [SerializeField] private InputAction _pointerPositionAction;
    [SerializeField] private InputAction _cancelAction;
    [SerializeField] private FortClickHandler _selectedFort;
    [SerializeField] private FlagCreatorEffects _effects;

    private float _height = 0f;
    private bool _isAwaitingPlacement = false;

    public bool IsFlagOnMap { get; private set; } = false;
    public Flag Flag { get; private set; }

    private void Awake()
    {
        Flag = Instantiate(_flagPrefab, transform.position, Quaternion.identity);
        Flag.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _clickAction.Enable();
        _clickAction.performed += OnClickPerformed;

        _pointerPositionAction.Enable();

        _cancelAction.Enable();
        _cancelAction.performed += OnCancelPerformed;

        _selectedFort.OnSelected += OnFortSelected;
    }

    private void OnDisable()
    {
        _clickAction.performed -= OnClickPerformed;
        _clickAction.Disable();

        _pointerPositionAction.Disable();

        _cancelAction.performed -= OnCancelPerformed;
        _cancelAction.Disable();

        _selectedFort.OnSelected -= OnFortSelected;
    }

    public void RemoveFlag()
    {
        Flag.gameObject.SetActive(false);
        IsFlagOnMap = false;
        Flag.ReadyToBeRemoved -= RemoveFlag;
    }

    private void OnFortSelected()
    {
        _isAwaitingPlacement = true;
        _effects.PlayFortSelectEffect();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        ExitPlacement();
        _effects.PlayCancelEffect();
    }

    private void ExitPlacement()
    {
        if (_isAwaitingPlacement)
        {
            _isAwaitingPlacement = false;
        }
    }

    private void PlaceFlag(Vector3 position)
    {
        Flag.gameObject.SetActive(true);
        position.y = _height;
        Flag.transform.position = position;
        _effects.PlayFlagPlacementEffect();
        IsFlagOnMap = true;
        Flag.ReadyToBeRemoved += RemoveFlag;
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (_isAwaitingPlacement == false)
        {
            return;
        }

        Vector2 screenPosition = _pointerPositionAction.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out var hit, _rayDistance, _groundMask))
        {
            PlaceFlag(hit.point);
            ExitPlacement();
        }
    }
}