using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class will enable and disable a panel from a button.
/// </summary>
public class HotSpotButton : MonoBehaviour
{
    #region Variables
    
    [Tooltip("The button that will enable and disable the hotspot panel.")]
    [SerializeField] private Button _button;
    [Tooltip("The panel that will be toggled with information about the hotspot.")]
    [SerializeField] private CanvasGroup _panelCanvasGroup;
    [Tooltip("The speed at which the panel should be lerped.")]
    [SerializeField] private float _speed = 0.2f;

    private bool _panelEnabled;
    private Coroutine _coroutine;
    
    #endregion
    
    #region Unity Callbacks
    
    /// <summary>
    /// Calls at startup.
    /// </summary>
    private void Start()
    {
        GetReferences();
        _button.onClick.AddListener(ToggleLerp);
        InputManager.Instance.leftMouseButtonClick += OnClickDisable;
    }

    #endregion
    
    #region Setup
    
    /// <summary>
    /// Sets any references that haven't been referenced.
    /// </summary>
    private void GetReferences()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
        
        if (_panelCanvasGroup == null)
        {
            _panelCanvasGroup = GetComponent<CanvasGroup>();
        }
        
        var buttonId = _button.gameObject.GetInstanceID();
        var panelId = _panelCanvasGroup.gameObject.GetInstanceID();

        if (panelId == buttonId)
        {
            Debug.LogError($"The panel and button reference are the same object. Ensure this is expected behaviour. \n GameObject: {gameObject.name} \n Script: HotSpotButton.cs");
        }
    }

    #endregion
    
    #region Lerp panel
    
    /// <summary>
    /// Toggles the panel between shown and hidden.
    /// </summary>
    private void ToggleLerp()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        if (_panelEnabled)
        {
            _coroutine = StartCoroutine(LerpPanel(0f));
        }
        else
        {
            _coroutine = StartCoroutine(LerpPanel(1f));
        }
    }

    /// <summary>
    /// Disables the panel when the user clicks selects something else.
    /// </summary>
    private void OnClickDisable()
    {
        if (!_panelEnabled)
        {
            return;
        }
        
        ToggleLerp();
    }
    
    /// <summary>
    /// Lerps the panel alpha.
    /// </summary>
    /// <param name="endAlpha"> The alpha it should be lerped to. </param>
    /// <returns></returns>
    private IEnumerator LerpPanel(float endAlpha)
    {
        var duration = _speed;
        var elapsed = 0f;
        
        var startAlpha = _panelCanvasGroup.alpha;

        while (elapsed < duration)
        {
            var normalizedTime = elapsed / duration;
            _panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _panelEnabled = !_panelEnabled;
        _panelCanvasGroup.alpha = endAlpha;
        _coroutine = null;
    }
    
    #endregion
}
