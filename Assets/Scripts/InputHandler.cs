using UnityEngine.InputSystem;
using UnityEngine;

public class InputHandler
{
    public Vector2 Position { get; private set; }
    public bool IsPressed { get; private set; }

    public void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Position = Touchscreen.current.primaryTouch.position.ReadValue();
            IsPressed = true;
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            Position = Mouse.current.position.ReadValue();
            IsPressed = true;
        }
        else
        {
            IsPressed = false;
        }
    }
}
