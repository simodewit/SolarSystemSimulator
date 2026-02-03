using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private PlanetModelButton _currentPlanet;
    private void Start()
    {
        InputManager.Instance.SpawnInstance();
    }
    public void LockToPlanet(PlanetModelButton planetToLockOn)
    {
        _currentPlanet = planetToLockOn;
    }

    private void LetPlanetGo()
    {

    }

    private void ToPlanetPOV()
    {

    }
}
