using Godot;

public partial class Player : CharacterBody2D
{
    //##########################################################


    [Export]
    public int MaxJumps { get; set; } = 1;

    [Export]
    public float JumpSpeed { get; set; } = 100.0f;

    [Export]
    public float DoubleJumpSpeed { get; set; } = 75.0f;

    [Export]
    public float gravity { get; set; } = 100.0f;

    [Export]
    public int speed { get; set; } = 200;

    //##########################################################

    int jumpCount = 0;

    Vector2 UP = Vector2.Up;

    private AnimatedSprite2D move;

    //##########################################################

    public override void _Ready()
    {
        move = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public void jumpAnim()
    {
        
        move.Play("jump");
    }

    private bool wasOnFloor = false;

    public override void _PhysicsProcess(double delta)
    {
        float direction = (Input.GetActionStrength("right") - Input.GetActionStrength("left"));
        Vector2 vel = Velocity;
        vel.X = direction * speed;
        if(!IsOnFloor())
            vel.Y += gravity * (float)delta;

        Velocity = vel;

        bool isFalling = Velocity.Y > 0 && !IsOnFloor();
        bool isJumping = Input.IsActionJustPressed("up") && IsOnFloor();
        bool isDoubleJumping = Input.IsActionJustPressed("up") && isFalling;
        bool isJumpCancelled = Input.IsActionJustReleased("up") && Velocity.Y < 0;
        bool isIdle = IsOnFloor() && Mathf.IsZeroApprox(vel.X);
        bool isRunning = !Mathf.IsZeroApprox(vel.X) && IsOnFloor();

        if (isJumping && jumpCount < MaxJumps)
        {
            jumpAnim();
            move.FlipH = vel.X < 0;
            vel.Y += -JumpSpeed;
            jumpCount++;
            GD.Print("Jumping, jump count: ", jumpCount);
        }
        else if (isDoubleJumping && jumpCount < MaxJumps)
        {
            jumpAnim();
            vel.Y += -DoubleJumpSpeed;
            jumpCount++;
            
        }
        else if (isJumpCancelled)
        {
            vel.Y = vel.Y * 0.75f;
            jumpAnim();
        }
        else if (wasOnFloor && IsOnFloor())
        {
            jumpCount = 0;
            if (isIdle)
            {
                move.Play("idle");
            }
            else if (isRunning)
            {
                
                move.Play("runRight");
                move.FlipH = vel.X < 0;
            }
        }
        wasOnFloor = IsOnFloor();

        Velocity = vel;
        MoveAndSlide();
    }
}