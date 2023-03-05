using Godot;
using System;

public partial class HUD : CanvasLayer
{

    [Signal]
    public delegate void StartGameEventHandler();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var button = GetNode<Button>("StartButton");
        button.Pressed += () => this.OnStartButtonPressed();
        GetNode<Timer>("MessageTimer").Timeout += () => this.OnMessageTimerTimeout();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void ShowMessage(string text)
    {
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        GetNode<Timer>("MessageTimer").Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");

        var messageTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, "timeout");
        var message = GetNode<Label>("Message");
        message.Text = "Dodge the Creeps!";
        message.Show();
        // await ToSignal(GetTree().CreateTimer(2), "timeout");
        GetNode<Button>("StartButton").Show();

    }

    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    public void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
    }

    public void OnMessageTimerTimeout()
    {
        GD.Print("I RUN");
        GetNode<Label>("Message").Hide();
    }
}
