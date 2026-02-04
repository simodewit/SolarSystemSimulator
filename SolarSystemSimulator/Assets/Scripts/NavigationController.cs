using System;
using UnityEngine;

/// <summary>
/// This controller is used in the galaxy scene to move around and observe the galaxy.
/// </summary>
public class NavigationController : MonoBehaviour
{
    #region Variables
    
    [Header("Spawning")]
    [Tooltip("The distance of the camera to the pivot point at startup.")]
    [SerializeField] private float _cameraDistance = 30f;
    
    [Header("Moving")]
    [Tooltip("This value can be null and will be defaulted to the transform this script is attached to. This is the point the camera will rotate around that will be moved around.")]
    [SerializeField] private Transform _pivotPoint;
    [Tooltip("The transform of the camera attached to the pivot point.")]
    [SerializeField] private Transform _camera;
    [Tooltip("The speed at which the controller moves.")]
    [SerializeField] private float _moveSpeed = 5f;
    [Tooltip("The speed at which the controller rotates.")]
    [SerializeField] private float _rotateSpeed = 30f;
    
    [Header("Scrolling")]
    [Tooltip("The speed at which the controller scrolls.")]
    [SerializeField] private float _scrollSpeed = 1f;
    [Tooltip("The minimum amount of space that the controller will keep between the pivot point and the camera.")]
    [SerializeField] private float _minimumScrollDistance = 1f;
    [Tooltip("The maximum amount of space that the controller will keep between the pivot point and the camera.")]
    [SerializeField] private float _maximumScrollDistance = 100f;
    
    [Header("Virtual box")]
    [Tooltip("The target place where the virtual box for out of bounds regulation will be created.")]
    [SerializeField] private Transform _virtualBoxTarget;
    [Tooltip("The size of the virtual box.")]
    [SerializeField] private Vector3 _virtualBoxSize = new(100f,50f,100f);

    [Tooltip("This action gets called every moment the controller uses the Pan() method.")]
    [HideInInspector] public Action isPanning;
    [Tooltip("You can add an planet size to this variable to ensure the controller can't scroll into the planet when examining.")]
    [HideInInspector] public float currentPlanetSize;
    
    private Bounds _virtualBox;
    private bool _enableMovement = true;
    
    #endregion
    
    #region Unity Callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        GetObjectReferences();
        CreateVirtualBox();
        SetCameraOffset();
    }

    /// <summary>
    /// Gets called every frame.
    /// </summary>
    private void Update()
    {
        if (_enableMovement)
        {
            Pan();
            Rotate();
            Zoom();
        }
        
        StayInBounds(); // Call this at the end to ensure the controller is clamped AFTER moving. (better experience)
    }
    
    #endregion

    #region Spawning

    /// <summary>
    /// Sets any references that haven't been referenced.
    /// </summary>
    private void GetObjectReferences()
    {
        if (_pivotPoint == null)
        {
            _pivotPoint = transform;
        }

        if (_camera == null)
        {
            _camera = transform;
        }

        var pivotPointID = _pivotPoint.gameObject.GetInstanceID();
        var cameraID = _camera.gameObject.GetInstanceID();

        if (pivotPointID == cameraID)
        {
            Debug.LogError($"WATCH OUT. You set the camera and pivot as the same reference, this WILL BREAK THE CONTROLLER. \n GameObject: {gameObject.name} \n Script: NavigationController.cs");
        }
    }
    
    /// <summary>
    /// Set the distance of the camera from the pivot point when spawning.
    /// </summary>
    private void SetCameraOffset()
    {
        if (_camera == null)
        {
            Debug.LogWarning($"Camera not set, cannot set it's position. \n GameObject: {gameObject.name} \n Script: NavigationController.cs");
            return;
        }
        
        _camera.localPosition = new Vector3(0,0,-_cameraDistance);
    }

    #endregion
    
    #region Virtual box

    /// <summary>
    /// Sets the variables of the minimum and maximum bounds of the virtual box that will keep the controller within bounds.
    /// </summary>
    private void CreateVirtualBox()
    {
        if (_virtualBoxTarget == null)
        {
            Debug.LogWarning($"Virtual box target not set, cannot create virtual box. \n GameObject: {gameObject.name} \n Script: NavigationController.cs");
            return;
        }
        
        var boxLocation = _virtualBoxTarget.position;
        var boxSize = _virtualBoxSize;
        
        _virtualBox = new Bounds(boxLocation, boxSize);
    }

    /// <summary>
    /// Keeps the controller within the bounds of the virtual box.
    /// </summary>
    private void StayInBounds()
    {
        if (_virtualBoxTarget == null)
        {
            Debug.LogWarning($"Virtual box target not set, cannot limit the bounds of the controller. \n GameObject: {gameObject.name} \n Script: NavigationController.cs");
            return;
        }
        
        var position = _pivotPoint.position;

        position.x = Mathf.Clamp(position.x, _virtualBox.min.x, _virtualBox.max.x);
        position.y = Mathf.Clamp(position.y, _virtualBox.min.y, _virtualBox.max.y);
        position.z = Mathf.Clamp(position.z, _virtualBox.min.z, _virtualBox.max.z);

        _pivotPoint.position = position;
    }
    
    #endregion
    
    #region Movement

    /// <summary>
    /// Toggles the movement of the controller on or off.
    /// </summary>
    /// <param name="value"></param>
    public void ToggleMovement(bool value)
    {
        _enableMovement = value;
    }
    
    /// <summary>
    /// Moves the camera through the scene.
    /// </summary>
    private void Pan()
    {
        var clickPerformed = InputManager.Instance.input.Mouse.LeftButton.IsPressed();
        
        if (!clickPerformed)
        {
            return;
        }
        
        isPanning?.Invoke();
        
        var mouseDelta = InputManager.Instance.input.Mouse.Delta.ReadValue<Vector2>();
        var speedCalculation = -mouseDelta * _moveSpeed;
        var movement = new Vector3(speedCalculation.x, speedCalculation.y, 0f) * Time.deltaTime;
        
        _pivotPoint.Translate(movement);
    }

    /// <summary>
    /// Rotates the camera around the pivot point.
    /// </summary>
    private void Rotate()
    {
        var clickPerformed = InputManager.Instance.input.Mouse.RightButton.IsPressed();
        
        if (!clickPerformed)
        {
            return;
        }
        
        var mouseDelta = InputManager.Instance.input.Mouse.Delta.ReadValue<Vector2>();
        var speedCalculation = mouseDelta * _rotateSpeed;
        var movement = new Vector3(-speedCalculation.y, speedCalculation.x, 0f) * Time.deltaTime;
        
        _pivotPoint.eulerAngles += movement;
    }

    /// <summary>
    /// Zooms the camera towards and away from the pivot point.
    /// </summary>
    private void Zoom()
    {
        var mouseDelta = InputManager.Instance.input.Mouse.Scroll.ReadValue<Vector2>();
        var correctedScrollDistance = Mathf.Max(currentPlanetSize, _minimumScrollDistance);
        
        var scrollAmount = mouseDelta.y * _scrollSpeed;
        var newPosition = _camera.localPosition.z + scrollAmount;
        newPosition = Mathf.Clamp(newPosition, -_maximumScrollDistance, -correctedScrollDistance);
        
        _camera.localPosition = new Vector3(_camera.localPosition.x,_camera.localPosition.y,newPosition);
    }
    
    #endregion
}
