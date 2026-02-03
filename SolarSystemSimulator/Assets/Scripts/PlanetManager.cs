using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private PlanetModelButton _currentPlanet;
    private Coroutine _cameraLerpCoroutine;
    [SerializeField] private Transform _camHolder;
    private Camera _cam;

    [SerializeField] private NavigationController _navigationController;

    [SerializeField] private CanvasGroup _galaxyPanel, _planetPanel;
    [SerializeField] private float _alphaLerpDuration;

    [Header("Camera Settings")]
    [SerializeField] private float _cameraLerpDuration;
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _distanceCamToPlanetFactor;
    private void Start()
    {
        InputManager.Instance.SpawnInstance();
        _navigationController.isPanning += LetPlanetGo;
        _cam = Camera.main;
    }
    public void LockToPlanet(PlanetModelButton planetToLockOn)
    {
        if (_currentPlanet != null)
        {
            LetPlanetGo();
        }
        _currentPlanet = planetToLockOn;

        //first stop cameralerpcoroutine
        if (_cameraLerpCoroutine != null)
            StopCoroutine(_cameraLerpCoroutine);

        _camHolder.SetParent(planetToLockOn.transform);
        _navigationController.ToggleMovement(false);
        _cameraLerpCoroutine = StartCoroutine(LerpCameraToTarget(_camHolder.transform.position, planetToLockOn.transform.position, _cam.transform.localPosition.z, GetCamZPosition()));
        StartCoroutine(LerpAlphaPanel(1,0,_galaxyPanel));
        StartCoroutine(LerpAlphaPanel(0,1,_planetPanel));
        StartCoroutine(LerpAlphaPanel(0,1,planetToLockOn.GetPlanetInfoPanel()));
    }

    private void LetPlanetGo()
    {
        if (_currentPlanet != null)
        {
            StopAllCoroutines();
            _camHolder.SetParent(null);
            StartCoroutine(LerpAlphaPanel(0,1,_galaxyPanel));
            StartCoroutine(LerpAlphaPanel(1,0, _planetPanel));
            StartCoroutine(LerpAlphaPanel(1, 0, _currentPlanet.GetPlanetInfoPanel()));
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
        // When the scene load script is made this function returns the selected planets scene index
    }

    private float GetCamZPosition()
    {
        Debug.Log(_currentPlanet.GetPlanetBounds());
        return _distanceCamToPlanetFactor * _currentPlanet.GetPlanetBounds();
    }
    private IEnumerator LerpCameraToTarget(Vector3 cameraHolderStartPos, Vector3 cameraHolderTargetPos, float cameraStartZPos, float cameraTargetZPos)
    {
        float elapsed = 0f;
        while (elapsed < _cameraLerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(0,1, elapsed / _cameraLerpDuration);
            _camHolder.position = Vector3.Lerp(cameraHolderStartPos, cameraHolderTargetPos, t);

            Vector3 localPos = _cam.transform.localPosition;
            localPos.z = Mathf.Lerp(cameraStartZPos, -cameraTargetZPos, t);
            _cam.transform.localPosition = localPos;

            yield return null;
        }

        // when lerp is finished
        // Ensure exact position at the end
        _camHolder.transform.position = cameraHolderTargetPos;
        
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
