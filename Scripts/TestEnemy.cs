using Godot;
using System;

public partial class TestEnemy : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 300.0f; // Speed of the enemy

    private Vector2 direction = Vector2.Right;
    private bool isChasingPlayer = false;
    private Timer timer;
    private static readonly float[] waitTimes = [3.0f, 5.0f, 6.0f];
    private static readonly Vector2[] potentialDirections =
    [
        Vector2.Right,   // Right
        Vector2.Left  // Left
    ];

    private AnimatedSprite2D animate;
    private RayCast2D collider;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // connect the timer's timeout signal to a method
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
        animate = GetNode<AnimatedSprite2D>("EnemyAnimatedSprite2D");
        collider = GetNode<RayCast2D>("RayCast2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (collider.IsColliding() && collider.GetCollider() is TileMapLayer)
        {
            collider.TargetPosition = -collider.TargetPosition; // Reverse the raycast direction if colliding with a TileMapLayer
            direction = -direction; // Reverse direction if colliding with a TileMapLayer
        }
        Move(delta);
    }

    private void AnimationHandler()
    {
        animate.FlipH = direction.X < 0; // Flip the sprite based on direction
        //animate.Play("run");
    }

    private void Move(double delta)
    {
        Vector2 velocity = Velocity;
        if (!isChasingPlayer)
        {
            velocity += direction * Speed * (float)delta;
            
        }

        AnimationHandler();

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnTimerTimeout()
    {
        timer.WaitTime = new GenericChoose<float>(waitTimes).Choose();
        if(!isChasingPlayer)
        {
            direction = new GenericChoose<Vector2>(potentialDirections).Choose();
        }
    }
}
