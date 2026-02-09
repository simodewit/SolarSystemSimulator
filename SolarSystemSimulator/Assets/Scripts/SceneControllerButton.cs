using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Triggers the SceneController to start loading in the next scene.
/// </summary>
public class SceneControllerButton : MonoBehaviour
{
    #region Variables
    
    [Tooltip("The button that triggers loading the next scene.")]
    [SerializeField] private Button _button;
    
    #endregion
    
    #region Unity Callbacks
    
    /// <summary>
    /// Gets called before the first rendered frame.
    /// </summary>
    private void Awake()
    {
        SceneController.Instance.SpawnInstance();
        GetReferences();
        SubscribeButton();
    }
    
    #endregion
    
    #region Getters
    
    /// <summary>
    /// Gets the references from the scene and sets the references to the SceneController.
    /// </summary>
    private void GetReferences()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
        
        if (_button == null)
        {
            Debug.LogWarning($"The button component cannot be found. \n GameObject: {gameObject.name} \n Script: SetSceneLoadImage.cs");
        }
    }
    
    #endregion
    
    #region Load scene
    
    /// <summary>
    /// Subscribes to the perspective button and calls to load the next scene.
    /// </summary>
    private void SubscribeButton()
    {
        _button.onClick.AddListener(LoadNextScene);
    }
    
    /// <summary>
    /// Fades the current scene out and fades the scene with the given index in. 
    /// </summary>
    private void LoadNextScene()
    {
        var sceneIndex = PlanetManager.Instance.ToPlanetPOV();
        SceneController.Instance.LoadScene(sceneIndex);
    }
    
    #endregion
}
