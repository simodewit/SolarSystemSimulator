using UnityEngine;
using System.Collections;

public class PlanetModelButton : MonoBehaviour
{
    [SerializeField] private PlanetManager _planetManager;

    [SerializeField] private int _sceneIndex;
    [SerializeField] private CanvasGroup _canvasGroup;

    
    [SerializeField] private Renderer _planetRenderer;
    [SerializeField] private Renderer _hologramRenderer;

    private float _hoverLerpDuration;

    private void Start()
    {
        _hoverLerpDuration = _planetManager.GetHoverLerpDuration();
    }
    public void OnPlanetClicked()
    {
        //Testing checking if function works
        Debug.Log($"{gameObject.name} is geselecteerd");
        _planetManager.LockToPlanet(this);
    }

    public void OnPlanetHover()
    {
        // Testing checking if function works
        Debug.Log($"{gameObject.name} is gehoverd.");
        StopAllCoroutines();
        StartCoroutine(HologramLerper(-0.05f, 1));
        // show planet hover shader
    }

    public void OnPlanetUnHover()
    {
        // Testing checking if function works
        Debug.Log($"{gameObject.name} is niet meer gehoverd.");
        StopAllCoroutines();
        StartCoroutine(HologramLerper(1, -0.05f));
        // hide planet hover shader
    }

    public int GetSceneIndex()
    {
        return _sceneIndex;
    }

    public CanvasGroup GetPlanetInfoPanel()
    {
        return _canvasGroup;
    }
    
    public float GetPlanetBounds()
    {
        Vector3 maxBounds = _planetRenderer.bounds.max;
        return maxBounds.y;
    }

    private IEnumerator HologramLerper(float start, float end)
    {
        float elapsed = 0f;
        
        while (elapsed < _hoverLerpDuration)
        {
            // Time calculations
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(0, 1, elapsed / _hoverLerpDuration);

            // Setting the hologram float
            float formingProgress = Mathf.Lerp(start, end, t);
            _hologramRenderer.material.SetFloat("_FormingProgress", formingProgress);

            yield return null;
        }

        // when lerp is finished
        // Ensure exact position at the end
        _hologramRenderer.material.SetFloat("_FormingProgress", end);
    }
}
