using UnityEngine;

public class PlanetModelButton : MonoBehaviour
{
    [SerializeField] private PlanetManager _planetManager;
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
}
