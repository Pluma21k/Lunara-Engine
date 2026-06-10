namespace Lunara2D.Core.Components;

public class LPlayerController : LComponent
{
    private float speed = 5.0f;
    private float gravity = 0.5f;
    private float velocityY = 0f;

    public override void Update(float dt)
    {
        var transform = Entity?.GetComponent<LTransformComponent>();

        // Basic Input (Check your input system implementation)
        if (LInputSystem.IsKeyDown(LInputSystem.LKeyCode.Right)) transform?.X += speed;
        if (LInputSystem.IsKeyDown(LInputSystem.LKeyCode.Left)) transform?.X -= speed;
        if (LInputSystem.IsKeyDown(LInputSystem.LKeyCode.Up)) transform?.Y -= speed;
        if (LInputSystem.IsKeyDown(LInputSystem.LKeyCode.Down)) transform?.Y += speed;

        // Simple Gravity
        //velocityY += gravity;
        //transform.Y += velocityY;

        // Floor Collision (Hardcoded for testing)
        if (transform?.Y > 600)
        {
            transform?.Y = 600;
            velocityY = 0;
        }
    }
}