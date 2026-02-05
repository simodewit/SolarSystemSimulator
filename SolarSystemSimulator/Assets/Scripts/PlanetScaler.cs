using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will scale the planets from pleasant to a relative realistic scale and back.
/// </summary>
public class PlanetScaler : MonoBehaviour
{
    #region variables
    
    [Tooltip("The button that will trigger scaling the planets.")]
    [SerializeField] private Button _button;
    [Tooltip("The time the lerp should take.")]
    [SerializeField] private float _lerpTime = 2f;

    [Tooltip("The references to each planet to scale.")] 
    [SerializeField] private PlanetScaleData[] _planets;
    
    private bool _isRealScale = false;
    
    #endregion
    
    #region unity callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        _button.onClick.AddListener(SwitchScale);
        SetScale();
    }
    
    #endregion
    
    #region Rotation

    /// <summary>
    /// This method will set the scale for the planets directly.
    /// </summary>
    private void SetScale()
    {
        if (_planets.Length == 0)
        {
            Debug.LogWarning($"There are no planets referenced. \n GameObject: {gameObject.name} \n Script: PlanetScaleData.cs");
            return;
        }
        
        foreach (var planet in _planets)
        {
            var newScale = new Vector3(planet.PleasantScale, planet.PleasantScale, planet.PleasantScale);
            planet.transform.localScale = newScale;
        }
    }
    
    /// <summary>
    /// Switches between the realistic scale and the pleasant scale.
    /// </summary>
    private void SwitchScale()
    {
        if (_isRealScale)
        {
            ScaleToPleasant();
        }
        else
        {
            ScaleToRealistic();
        }
        
        _isRealScale = !_isRealScale;
    }

    #endregion
    
    #region To realistic
    
    /// <summary>
    /// This will start the coroutine for every planet at the same time to scale at the same moment.
    /// </summary>
    private void ScaleToRealistic()
    {
        foreach (var planet in _planets)
        {
            StartCoroutine(ScaleToRealistic(planet));
        }
    }

    /// <summary>
    /// This method will start the planet scaling to get the scale to a realistic size relative to the other planets.
    /// </summary>
    /// <param name="planet"> The planet to scale. </param>
    /// <returns></returns>
    private IEnumerator ScaleToRealistic(PlanetScaleData planet)
    {
        var modelRenderer = planet.modelRenderer;
        
        var boundSize = modelRenderer.bounds.size;
        var desiredBoundSize = planet.ConvertedBounds;
        var scale = planet.transform.localScale;

        var scaleMultiplier = desiredBoundSize / boundSize.y;
        var newScale = scale * scaleMultiplier;

        var duration = _lerpTime;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            var time = elapsed / duration;

            planet.transform.localScale = Vector3.Lerp(scale, newScale, time);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        planet.transform.localScale = newScale;
    }
    
    #endregion

    #region To pleasant
    
    /// <summary>
    /// Scales the planets to the same size.
    /// </summary>
    private void ScaleToPleasant()
    {
        foreach (var planet in _planets)
        {
            StartCoroutine(ScaleToPleasant(planet));
        }
    }
    
    /// <summary>
    /// This method will start the planet scaling to get the scale to a pleasant size to look at.
    /// </summary>
    /// <param name="planet"> The planet to scale. </param>
    /// <returns></returns>
    private IEnumerator ScaleToPleasant(PlanetScaleData planet)
    {
        var scale = planet.transform.localScale;
        var pleasantScale = planet.PleasantScale;
        var newScale = new Vector3(pleasantScale, pleasantScale, pleasantScale);

        var duration = _lerpTime;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            var time = elapsed / duration;

            planet.transform.localScale = Vector3.Lerp(scale, newScale, time);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        planet.transform.localScale = newScale;
    }
    
    #endregion
}
