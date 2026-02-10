using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class StartZoom : MonoBehaviour
{
    #region Variables
    
    [Tooltip("The duration of the zoom.")]
    [SerializeField] private float _time = 2f;
    [Tooltip("The position it should move towards.")]
    [SerializeField] private Vector3 _position;
    [Tooltip("The rotation it should move towards.")]
    [SerializeField] private Vector3 _rotation;
    
    #endregion
    
    #region Unity callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        StartCoroutine(Zoom());
    }

    #endregion
    
    #region Zoom
    
    /// <summary>
    /// Lerps the camera to the desired location and rotation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Zoom()
    {
        var controller = NavigationController.Instance;

        var startPosition = controller.transform.position;
        var startRotation = controller.transform.eulerAngles;
        
        var endPosition = _position;
        var endRotation = _rotation;
        
        var duration = _time;
        var elapsed = 0f;

        controller.ToggleMovement(false);
        
        while (elapsed < duration)
        {
            var normalizedTime = elapsed / duration;

            controller.transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);
            controller.transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, normalizedTime);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        controller.transform.position = endPosition;
        controller.transform.eulerAngles = endRotation;
        
        controller.ToggleMovement(true);
    }
    
    #endregion
}
