using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [HideInInspector] public MainInputs input;
    private Camera _cam;
    private PlanetModelButton _currentHover;

    protected override void Awake()
    {
        base.Awake();

        input = new MainInputs();
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Mouse.LeftButton.performed += OnClick;
        //input.Mouse.Position
    }

    private void OnDisable()
    {
        input.Mouse.LeftButton.performed -= OnClick;
        input.Disable();
    }

    private void Update()
    {
        HandleHover();
    }

    private void HandleHover()
    {
        // Ray from camera to mouse
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if hit object has PlanetModelButton
            PlanetModelButton hoverButton = hit.collider.GetComponent<PlanetModelButton>();

            if (hoverButton != null)
            {
                if (_currentHover != hoverButton)
                {
                    // Hover exited old
                    _currentHover?.OnPlanetUnHover();

                    // Hover entered new
                    _currentHover = hoverButton;
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

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
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
}
