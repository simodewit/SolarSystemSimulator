using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This class will let any object rotate.
/// </summary>
public class ObjectRotater : MonoBehaviour
{
    #region Variables
    
    [Tooltip("The target transform that will be rotated.")]
    [SerializeField] private Transform _target;

    [Header("Axis")]
    [Tooltip("The speed at which the target will be rotated.")]
    [SerializeField] private float _xSpeed = 0;
    [Tooltip("If turned on this script will rotate around the Y axis.")]
    [SerializeField] private float _ySpeed = 5f;
    [Tooltip("If turned on this script will rotate around the Z axis.")]
    [SerializeField] private float _zSpeed = 0;

    [Header("Multiplier")] 
    [Tooltip("Multiplies the speeds. Useful to work with small or big units to convert them to your wanted speeds.")] 
    [SerializeField] private float _multiplier = 1f;
    
    #endregion
    
    #region Unity Callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        GetObjectReferences();
    }
    
    /// <summary>
    /// Gets called every frame.
    /// </summary>
    private void Update()
    {
        RotatePlanet();
    }
    
    #endregion
    
    #region Setup
    
    /// <summary>
    /// Sets any references that haven't been referenced.
    /// </summary>
    private void GetObjectReferences()
    {
        if (_target == null)
        {
            _target = transform;
        }
    }
    
    #endregion

    #region Rotate
    
    /// <summary>
    /// Rotates the planet with a set speed.
    /// </summary>
    private void RotatePlanet()
    {
        var rotation = new Vector3(0, 0, 0);
        
        rotation.x = _xSpeed * _multiplier * Time.deltaTime;
        rotation.y = _ySpeed * _multiplier * Time.deltaTime;
        rotation.z = _zSpeed * _multiplier * Time.deltaTime;
        
        _target.Rotate(rotation);
    }
    
    #endregion
}
