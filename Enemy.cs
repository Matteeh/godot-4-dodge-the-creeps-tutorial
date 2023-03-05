using Godot;
using System;

public partial class Enemy : RigidBody2D
{
    public override void _Ready()
    {
        var animSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animSprite2D.Play();
        string[] mobTypes = animSprite2D.SpriteFrames.GetAnimationNames();
        animSprite2D.Animation = mobTypes[GD.Randi() % mobTypes.Length];
        var outsideScreen = GetNode<VisibleOnScreenEnabler2D>("VisibleOnScreenEnabler2D");
        outsideScreen.ScreenExited += () => this.OnVisibleOnScreenNotifier2DScreenExited();
    }

    public void OnVisibleOnScreenNotifier2DScreenExited()
    {
        GD.Print("ON END OF SCREEN");
        QueueFree();
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
