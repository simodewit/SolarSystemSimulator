using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A singleton manager that can always be called. This manager provides all inputs from the new input system.
/// </summary>
public class InputManager : Singleton<InputManager>
{
    [Tooltip("This is where you will be able to find all your inputs and action maps.")]
    [HideInInspector] public MainInputs input;
    
    private Camera _camera;
    private PlanetModelButton _currentHover;

    #region Unity callbacks

    /// <summary>
    /// Gets called before the first rendered frame.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // Call to ensure the singleton awake gets called.

        input = new MainInputs();
        _camera = Camera.main;
    }

    /// <summary>
    /// Gets called when a user clicks back into the application.
    /// </summary>
    private void OnEnable()
    {
        input.Enable();
        input.Mouse.LeftButton.performed += OnClick;
    }

    /// <summary>
    /// Gets called when the user clicks out of this application.
    /// </summary>
    private void OnDisable()
    {
        input.Mouse.LeftButton.performed -= OnClick;
        input.Disable();
    }

    /// <summary>
    /// Gets called every frame.
    /// </summary>
    private void Update()
    {
        HandleHover();
    }
    
    #endregion

    #region Planet selection
    
    /// <summary>
    /// Checks if the user hovers over a planet.
    /// </summary>
    private void HandleHover()
    {
        // Ray from camera to mouse
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if hit object has PlanetModelButton
            PlanetModelButton planetHit = hit.collider.GetComponent<PlanetModelButton>();

            if (planetHit != null)
            {
                if (_currentHover != planetHit)
                {
                    // Hover exited old
                    _currentHover?.OnPlanetUnHover();

                    // Hover entered new
                    _currentHover = planetHit;
                    _currentHover.OnPlanetHover();
                }
                return;
            }
        }

        // No object hit, clear current hover
        if (_currentHover != null)
        {
            _currentHover.OnPlanetUnHover();
            _currentHover = null;
        }
    }

    /// <summary>
    /// Checks if the user clicks on a planet.
    /// </summary>
    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.SendMessage("OnClicked", SendMessageOptions.DontRequireReceiver);
            PlanetModelButton planetHit = hit.transform.GetComponent<PlanetModelButton>();
            if (planetHit != null)
            {
                planetHit.OnPlanetClicked();
            }
        }
    }
    
    #endregion
}
