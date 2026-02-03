using UnityEngine;

public class PlanetModelButton : MonoBehaviour
{
    [SerializeField] private PlanetManager _planetManager;

    [SerializeField] private int _sceneIndex;
    [SerializeField] private CanvasGroup _canvasGroup;

    
    [SerializeField] private Renderer _planetRenderer;
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

        // show planet hover shader
    }

    public void OnPlanetUnHover()
    {
        // Testing checking if function works
        Debug.Log($"{gameObject.name} is niet meer gehoverd.");

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
        return Mathf.Max( maxBounds.x, maxBounds.y, maxBounds.z );
    }

}
