using System.Collections;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private PlanetModelButton _currentPlanet;
    [SerializeField] private Transform _camHolder;

    [SerializeField] private NavigationController _navigationController;

    [SerializeField] private CanvasGroup _galaxyPanel, _planetPanel;
    [SerializeField] private float _alphaLerpDuration;

    [Header("Camera Settings")]
    [SerializeField] private float _cameraLerpDuration;
    [SerializeField] private float _cameraOffset;
    private void Start()
    {
        InputManager.Instance.SpawnInstance();
        _navigationController.isPanning += LetPlanetGo;
    }
    public void LockToPlanet(PlanetModelButton planetToLockOn)
    {
        _currentPlanet = planetToLockOn;

        //first stop all coroutines before starting the new one
        StopAllCoroutines();

        _camHolder.SetParent(planetToLockOn.transform);
        _navigationController.ToggleMovement(false);
        Coroutine cameraLerp = StartCoroutine(LerpCameraToTarget(_camHolder.transform.position, planetToLockOn.transform.position));
        StartCoroutine(LerpAlphaPanel(1,0,_galaxyPanel));
        StartCoroutine(LerpAlphaPanel(0,1,_planetPanel));
    }

    private void LetPlanetGo()
    {
        if (_currentPlanet != null)
        {
            StopAllCoroutines();
            _camHolder.SetParent(null);
            StartCoroutine(LerpAlphaPanel(0,1,_galaxyPanel));
            StartCoroutine(LerpAlphaPanel(1,0, _planetPanel));
            _currentPlanet = null;
        }
    }

    public int ToPlanetPOV()
    {
        if (_currentPlanet != null)
        {
            return _currentPlanet.GetSceneIndex();
        }
        else
        {
            Debug.LogError("There is no planet selected");
            return 0;
        }
        // Wanneer het script dat de scene laad komt laad je hiermee de goede scene in
    }

    private IEnumerator LerpCameraToTarget(Vector3 cameraStartPos, Vector3 cameraTargetPos)
    {
        float elapsed = 0f;

        while (elapsed < _cameraLerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(0,1, elapsed / _cameraLerpDuration);
            _camHolder.position = Vector3.Lerp(cameraStartPos, cameraTargetPos, t);

            yield return null;
        }

        // when lerp is finished
        // Ensure exact position at the end
        _camHolder.transform.position = cameraTargetPos;
        
        _navigationController.ToggleMovement(true);
    }

    private IEnumerator LerpAlphaPanel(float alphaStart, float alphaEnd, CanvasGroup groupToLerp)
    {
        float elapsed = 0f;
        if (alphaEnd == 0)
        {
            groupToLerp.interactable = false;
        }
        else
        {
            groupToLerp.interactable = true;
        }
            while (elapsed < _alphaLerpDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Lerp(0, 1, elapsed / _cameraLerpDuration);
                groupToLerp.alpha = Mathf.Lerp(alphaStart, alphaEnd, t);

                yield return null;
            }

        // when lerp is finished
        // Ensure exact position at the end
        groupToLerp.alpha = alphaEnd;
    }
}
