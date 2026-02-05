using UnityEngine;

/// <summary>
/// Data for the planet on how big it should be in different modes.
/// </summary>
public class PlanetScaleData : MonoBehaviour
{
    #region Variables
    
    [Tooltip("The transform of the object that will be scaled.")]
    public Transform scaleTransform;
    [Tooltip("The renderer of the model.")]
    public Renderer modelRenderer;
    
    [Tooltip("The real world diameter of the planet in kilometers.")]
    [SerializeField] private float _diameter = 12765f;
    [Tooltip("The size of the planet bounds when in pleasant mode.")]
    [SerializeField] private float _planetPleasantScale = 1f;

    private const float _divider = 100000f;

    #endregion
    
    #region Unity Callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        GetObjectReferences();
    }
    
    #endregion
    
    #region Setup
    
    /// <summary>
    /// Sets any references that haven't been referenced.
    /// </summary>
    private void GetObjectReferences()
    {
        if (scaleTransform == null)
        {
            scaleTransform = transform;
        }

        if (modelRenderer == null)
        {
            modelRenderer = GetComponent<Renderer>();
        }
        
        var scaleTransformID = scaleTransform.gameObject.GetInstanceID();
        var rendererID = modelRenderer.gameObject.GetInstanceID();

        if (scaleTransformID == rendererID)
        {
            Debug.LogError($"WATCH OUT. You set the transform and renderer as the same reference, this WILL BREAK THE CONTROLLER. \n GameObject: {gameObject.name} \n Script: PlanetScaleData.cs");
        }
    }

    #endregion
    
    #region Data
    
    /// <summary>
    /// Returns the real size of the planet corrected to a usable value.
    /// </summary>
    public float ConvertedBounds => _diameter / _divider;

    /// <summary>
    /// Returns the scale the model should be when in the pleasant mode.
    /// </summary>
    public float PleasantScale => _planetPleasantScale;
    
    #endregion
}
