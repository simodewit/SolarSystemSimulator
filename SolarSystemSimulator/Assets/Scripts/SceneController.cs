using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script will load from the main scene to a specific planet scene and back.
/// </summary>
public class SceneController : Singleton<SceneController>
{
    #region Variables
    
    private const float _lerpSpeed = 2f;
    private const float _delayTime = 0.5f;
    private Image _image;
    
    #endregion
    
    #region Unity Callbacks

    private void Start()
    {
        CreateCanvas();
    }
    
    #endregion
    
    #region Setup

    private void CreateCanvas()
    {
        var canvasObject = new GameObject("FadingCanvas");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        
        var imageObject = new GameObject("FadingImage");
        imageObject.transform.SetParent(imageObject.transform);
        
        var image = imageObject.AddComponent<Image>();
        var imageTransform = image.GetComponent<RectTransform>();
        
        imageTransform.anchorMin = Vector2.zero;
        imageTransform.anchorMax = Vector2.one;
        imageTransform.offsetMin = Vector2.zero;
        imageTransform.offsetMax = Vector2.zero;
        imageTransform.pivot = new Vector2(0.5f, 0.5f);
        
        imageTransform.anchoredPosition = Vector2.zero;
        
        canvasObject.transform.SetParent(transform);
        _image = image;
    }
    
    #endregion
    
    #region Load next scene
    
    /// <summary>
    /// Fades the current scene out and fades the scene with the given index in.
    /// </summary>
    /// <param name="sceneIndex"> The index of the scene to load. </param>
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(FadeOutScene(sceneIndex));
    }
    
    /// <summary>
    /// Loads the scene from the given index.
    /// </summary>
    /// <param name="sceneIndex"> The index of the scene to load. </param>
    private IEnumerator LoadNextScene(int sceneIndex)
    {
        var elapsed = 0f;
        var sceneReady = false;

        var operation = SceneManager.LoadSceneAsync(sceneIndex);

        if (operation != null)
        {
            operation.allowSceneActivation = false;

            while (!sceneReady || elapsed < _delayTime)
            {
                if (operation.progress >= 0.9f)
                {
                    sceneReady = true;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            
            operation.allowSceneActivation = true;
        }
        
        StartCoroutine(FadeInScene());
    }
    
    #endregion
    
    #region Fading
    
    /// <summary>
    /// Fades from the scene into a black screen to seamlessly transition between scenes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutScene(int sceneIndex)
    {
        if (_image == null)
        {
            Debug.LogWarning($"The image doesn't have a reference, fading skipped. \n GameObject: {gameObject.name} \n Script: SceneController.cs");
            yield break;
        }
        
        var duration = _lerpSpeed / 2f;
        var elapsed = 0f;
        
        _image.color = Color.clear;
        _image.enabled = true;

        while (elapsed < duration)
        {
            var time = elapsed / duration;
            _image.color = Color.Lerp(Color.clear, Color.black, time);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _image.color = Color.black;
        
        StartCoroutine(LoadNextScene(sceneIndex));
    }
    
    /// <summary>
    /// Fades from a black screen to the scene.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInScene()
    {
        if (_image == null)
        {
            Debug.LogWarning($"The image doesn't have a reference, fading skipped. \n GameObject: {gameObject.name} \n Script: SceneController.cs");
            yield break;
        }

        var duration = _lerpSpeed / 2f;
        var elapsed = 0f;
        
        _image.color = Color.black;
        _image.enabled = true;
        
        while (elapsed < duration)
        {
            var time = elapsed / duration;
            _image.color = Color.Lerp(Color.black, Color.clear, time);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _image.color = Color.clear;
        _image.enabled = false;
    }
    
    #endregion
}
