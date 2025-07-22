using Godot;

public partial class Player : CharacterBody2D
{

    [Export]
    public int Speed { get; set; } = 200;

    [Export]
    public int JumpSpeed { get; set; } = 200;
    [Export]
    public float Gravity { get; set; } = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    bool isJumping = true;

    private AnimatedSprite2D move;

    public override void _Ready()
    {
        move = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public Vector2 getInput()
    {
        // Check for input and set the animation accordingly
        int inputDirection = 0;
        int jump = 0;
        Vector2 vector;
        if (Input.IsActionPressed("right"))
        {
            inputDirection = 1;
            move.Play("runRight");
        }
        else if (Input.IsActionPressed("left"))
        {
            inputDirection = -1;
            move.Play("runLeft");
        }
        else if (Input.IsActionPressed("up"))
        {
            if (!isJumping)
            {
                isJumping = true;
                jump = -JumpSpeed;
                move.Play("jump");
            }
            else
            {
                jump = 0;
            }
        }
        else
        {
            inputDirection = 0;
            move.Play("idle");
        }

        vector = new Vector2(Speed * inputDirection, jump);
        // If no input is detected, return a zero vector
        return (vector);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 pos = getInput();
        
        if(isJumping)
        {
            pos.Y += Gravity * (float)delta;
            //move.Play("fall");
        }
            Position += pos * (float)delta;

    }
}