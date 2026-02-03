using UnityEngine;

public class PlanetModelButton : MonoBehaviour
{
    private void OnPlanetClicked()
    {

    }

    private void OnPlanetHover()
    {
        // Testing kijken of de functie werkt
        Debug.Log($"{gameObject.name} is gehoverd.");

        // Laat het planeet hover effect zien.
    }

    private void OnPlanetUnHover()
    {
        // Testing kijken of de functie werkt
        Debug.Log($"{gameObject.name} is niet meer gehoverd.");

        // Zet het planeet hover effect uit.
    }
}
