using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [HideInInspector] public MainInputs input;
    
    private void Awake()
    {
        input = new MainInputs();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
