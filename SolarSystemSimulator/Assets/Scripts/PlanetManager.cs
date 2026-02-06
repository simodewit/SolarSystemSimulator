using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetManager : MonoBehaviour
{
    private PlanetModelButton _currentPlanet;
    private Coroutine _cameraLerpCoroutine;
    [SerializeField] private Transform _camHolder;
    private Camera _cam;

    [SerializeField] private CanvasGroup _galaxyCanvasGroup, _planetCanvasGroup;
    [SerializeField] private float _alphaLerpDuration;

    [Header("Camera Settings")]
    [SerializeField] private float _cameraLerpDuration;
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _distanceCamToPlanetFactor;

    [SerializeField] private float _hoverEffectLerpDuration;

    [HideInInspector] public static PlanetManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        InputManager.Instance.SpawnInstance();
        NavigationController.Instance.isPanning += LetPlanetGo;
        _cam = Camera.main;
    }
    public void LockToPlanet(PlanetModelButton planetToLockOn)
    {
        if (_currentPlanet != null)
        {
            LetPlanetGo();
        }
        _currentPlanet = planetToLockOn;

        // Turn planet hover efffect off
        _currentPlanet.OnPlanetUnHover();
        InputManager.Instance.ToggleHover(false);

        //first stop cameralerpcoroutine
        if (_cameraLerpCoroutine != null)
            StopCoroutine(_cameraLerpCoroutine);

        _camHolder.SetParent(planetToLockOn.transform);

        // Stop panning when lerping the camera
        NavigationController.Instance.ToggleMovement(false);

        // Camera lerper
        _cameraLerpCoroutine = StartCoroutine(LerpCameraToTarget(_camHolder.transform.position, planetToLockOn.transform, _cam.transform.localPosition.z, GetCamZPosition()));

        // Panel lerpers
        StartCoroutine(LerpAlphaPanel(1,0,_galaxyCanvasGroup));
        StartCoroutine(LerpAlphaPanel(0,1,_planetCanvasGroup));
        StartCoroutine(LerpAlphaPanel(0,1,planetToLockOn.GetPlanetInfoPanel()));
    }

    private void LetPlanetGo()
    {
        if (_currentPlanet != null)
        {
            StopAllCoroutines();

            _camHolder.SetParent(null);

            // Panel lepers
            StartCoroutine(LerpAlphaPanel(0,1,_galaxyCanvasGroup));
            StartCoroutine(LerpAlphaPanel(1,0, _planetCanvasGroup));
            StartCoroutine(LerpAlphaPanel(1, 0, _currentPlanet.GetPlanetInfoPanel()));

            _currentPlanet = null;

            // Hover effect is triggerable
            InputManager.Instance.ToggleHover(true);
        }
    }

    public int ToPlanetPOV()
    {
        InputManager.Instance.ToggleHover(true);
        if (_currentPlanet != null)
        {
            return _currentPlanet.GetSceneIndex();
        }
        else
        {
            return 0;
        }
        // When the scene load script is made this function returns the selected planets scene index
    }

    private float GetCamZPosition()
    {
        return _distanceCamToPlanetFactor * _currentPlanet.GetPlanetBounds();
    }

    public float GetHoverLerpDuration()
    {
        return _hoverEffectLerpDuration;
    }
    private IEnumerator LerpCameraToTarget(Vector3 cameraHolderStartPos, Transform planetTargetCameraHolder, float cameraStartZPos, float cameraTargetZPos)
    {
        float elapsed = 0f;
        while (elapsed < _cameraLerpDuration)
        {
            // Time calculations
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(0,1, elapsed / _cameraLerpDuration);

            // camholder lerp
            _camHolder.position = Vector3.Lerp(cameraHolderStartPos, planetTargetCameraHolder.position, t);

            // cam lerp (zoom)
            Vector3 localPos = _cam.transform.localPosition;
            localPos.z = Mathf.Lerp(cameraStartZPos, -cameraTargetZPos, t);
            _cam.transform.localPosition = localPos;

            yield return null;
        }

        // when lerp is finished
        // Ensure exact position at the end
        _camHolder.transform.position = planetTargetCameraHolder.position;
        
        // Panning is available when lerp ends
        NavigationController.Instance.ToggleMovement(true);
    }

    private IEnumerator LerpAlphaPanel(float alphaStart, float alphaEnd, CanvasGroup groupToLerp)
    {
        float elapsed = 0f;
        if (alphaEnd == 0)
        {
            groupToLerp.blocksRaycasts = false;
        }
        else
        {
            groupToLerp.blocksRaycasts = true;
        }
            while (elapsed < _alphaLerpDuration)
            {
                // Time calculations
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
