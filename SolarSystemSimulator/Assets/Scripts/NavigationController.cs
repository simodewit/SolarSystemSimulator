using System;
using UnityEngine;

/// <summary>
/// This controller is used in the galaxy scene to move around and observe the galaxy.
/// </summary>
public class NavigationController : MonoBehaviour
{
    [Header("Moving")]
    [Tooltip("This value can be null and will be defaulted to the transform this script is attached to. This is the point the camera will rotate around that will be moved around.")]
    [SerializeField] private Transform _pivotPoint;
    [Tooltip("The speed at which the controller will be moving.")]
    [SerializeField] private float _speed;
    
    [Header("Virtual box")]
    [Tooltip("The target place where the virtual box for out of bounds regulation will be created.")]
    [SerializeField] private Transform _virtualBoxTarget;
    [Tooltip("The size of the virtual box.")]
    [SerializeField] private Vector3 _virtualBoxSize = new(30f,10f,30f);

    [HideInInspector] public Action isPanning;
    
    private Bounds _virtualBox;
    private bool _enableMovement;
    
    private void Start()
    {
        CreateVirtualBox();

        if (_pivotPoint == null)
        {
            _pivotPoint = transform;
        }
    }

    private void Update()
    {
        Pan();
        Rotate();
        Zoom();
        
        StayInBounds(); // Call this at the end to ensure the controller is clamped AFTER moving. (better experience)
    }

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

    public void ToggleMovement(bool value)
    {
        _enableMovement = value;
    }
    
    private void Pan()
    {
        if (!_enableMovement)
        {
            return;
        }
    }

    private void Rotate()
    {
        if (!_enableMovement)
        {
            return;
        }
    }

    private void Zoom()
    {
        if (!_enableMovement)
        {
            return;
        }
    }
}
