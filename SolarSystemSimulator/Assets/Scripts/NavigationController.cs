using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This controller is used in the galaxy scene to move around and observe the galaxy.
/// </summary>
public class NavigationController : MonoBehaviour
{
    #region Variables
    
    [Header("Spawning")]
    [Tooltip("The distance of the camera to the pivot point at startup.")]
    [SerializeField] private float _cameraDistance = 5f;
    [Tooltip("The position of the pivot point at startup.")]
    [SerializeField] private Vector3 _pivotPointPosition = new(0f,0f,0f);
    [Tooltip("The rotation of the pivot point at startup.")]
    [SerializeField] private Vector3 _pivotPointRotation = new(0.5f,0.5f,0.5f);
    
    [Header("Moving")]
    [Tooltip("This value can be null and will be defaulted to the transform this script is attached to. This is the point the camera will rotate around that will be moved around.")]
    [SerializeField] private Transform _pivotPoint;
    [Tooltip("The transform of the camera attached to the pivot point.")]
    [SerializeField] private Transform _camera;
    [Tooltip("The speed at which the controller moves.")]
    [SerializeField] private float _moveSpeed = 5f;
    [Tooltip("The speed at which the controller rotates.")]
    [SerializeField] private float _rotateSpeed = 5f;
    [Tooltip("The speed at which the controller scrolls.")]
    [SerializeField] private float _scrollSpeed = 5f;
    
    [Header("Virtual box")]
    [Tooltip("The target place where the virtual box for out of bounds regulation will be created.")]
    [SerializeField] private Transform _virtualBoxTarget;
    [Tooltip("The size of the virtual box.")]
    [SerializeField] private Vector3 _virtualBoxSize = new(30f,10f,30f);

    [HideInInspector] public Action isPanning;
    
    private Bounds _virtualBox;
    private bool _enableMovement;
    
    #endregion
    
    #region Unity Callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        CreateVirtualBox();

        if (_pivotPoint == null)
        {
            _pivotPoint = transform;
        }

        SetCameraOffset();
        SetPivotPointOffset();
    }

    /// <summary>
    /// Updates every frame.
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
    /// Set the distance of the camera from the pivot point when spawning.
    /// </summary>
    private void SetCameraOffset()
    {
        _camera.position = new Vector3(0,0,_cameraDistance);
    }

    /// <summary>
    /// Set the position and rotation offset of the pivot point when spawning.
    /// </summary>
    private void SetPivotPointOffset()
    {
        _pivotPoint.position = _pivotPointPosition;
        _pivotPoint.eulerAngles = _pivotPointRotation;
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
            Debug.Log($"Virtual box target not set, cannot create virtual box. \n GameObject: {gameObject.name} \n Script: NavigationController.cs");
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
            Debug.Log($"Virtual box target not set, cannot limit the bounds of the controller. \n GameObject: {gameObject.name} \n Script: NavigationController.cs");
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
        
        var mouseDelta = InputManager.Instance.input.Mouse.Delta.ReadValue<Vector2>();
        var speedCalculation = mouseDelta * _moveSpeed;
        var movement = new Vector3(speedCalculation.x, speedCalculation.y, 0f);

        _pivotPoint.position += movement;
    }

    /// <summary>
    /// Rotates the camera around the pivot point.
    /// </summary>
    private void Rotate()
    {
        var mouseDelta = InputManager.Instance.input.Mouse.Delta.ReadValue<Vector2>();
    }

    /// <summary>
    /// Zooms the camera towards and away from the pivot point.
    /// </summary>
    private void Zoom()
    {
        var mouseDelta = InputManager.Instance.input.Mouse.Delta.ReadValue<Vector2>();
    }
    
    #endregion
}
