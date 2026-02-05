using UnityEngine;

/// <summary>
/// Data for the planet on how big it should be in different modes.
/// </summary>
public class PlanetScaleData : MonoBehaviour
{
    [Tooltip("The real world diameter of the planet in kilometers.")]
    [SerializeField] private float _diameter = 12765f;
    [Tooltip("The size of the planet bounds when in pleasant mode.")]
    [SerializeField] private float _planetPleasantScale = 1f;

    private const float _divider = 100000f;
    
    /// <summary>
    /// Returns the real size of the planet corrected to a usable value.
    /// </summary>
    public float ConvertedBounds => _diameter / _divider;

    /// <summary>
    /// Returns the scale the model should be when in the pleasant mode.
    /// </summary>
    public float PleasantScale => _planetPleasantScale;
}
