using Godot;
using System;

public partial class Main : Node
{


    [Export]
    public PackedScene EnemyScene;

    public int Score;

    public override void _Ready()
    {
        var hud = GetNode<HUD>("HUD");
        hud.StartGame += () => NewGame();

    }

    public void GameOver()
    {
        GetNode<AudioStreamPlayer2D>("DeathSound").Play();
        GetNode<AudioStreamPlayer2D>("Music").Stop();
        GetNode<Timer>("EnemyTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<HUD>("HUD").ShowGameOver();
    }

    public void NewGame()
    {
        GetTree().CallGroup("enemies", "queue_free");
        GetNode<AudioStreamPlayer2D>("Music").Play();

        // Note that for calling Godot-provided methods with strings,
        // we have to use the original Godot snake_case name.
        Score = 0;

        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(Score);
        hud.ShowMessage("Get Ready!");

        var player = GetNode<Player>("Player");
        player.Hit += () => this.GameOver();
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        Timer startTimer = this.GetNode<Timer>("StartTimer");
        startTimer.Start();
        startTimer.Timeout += () => this.OnStartTimerTimeout();
    }

    public void OnScoreTimerTimeout()
    {
        Score++;
        GetNode<HUD>("HUD").UpdateScore(Score);
    }

    public void OnStartTimerTimeout()
    {
        var enemyTimer = GetNode<Timer>("EnemyTimer");
        enemyTimer.Start();
        enemyTimer.Timeout += () => this.OnEnemyTimerTimeout();

        var scoreTimer = GetNode<Timer>("ScoreTimer");
        scoreTimer.Start();
        scoreTimer.Timeout += () => this.OnScoreTimerTimeout();

    }

    public void OnEnemyTimerTimeout()
    {
        // Note: Normally it is best to use explicit types rather than the `var`
        // keyword. However, var is acceptable to use here because the types are
        // obviously Mob and PathFollow2D, since they appear later on the line.

        // Create a new instance of the enemy scene.
        var enemy = EnemyScene.Instantiate<Enemy>();

        // Choose a random location on Path2D.
        var enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");
        enemySpawnLocation.ProgressRatio = GD.Randi();

        // Set the mob's direction perpendicular to the path direction.
        float direction = enemySpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        enemy.Position = enemySpawnLocation.Position;

        // Add some randomness to the direction.
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        enemy.Rotation = direction;

        // Choose the velocity.
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        enemy.LinearVelocity = velocity.Rotated(direction);

        // Spawn the mob by adding it to the Main scene.
        AddChild(enemy);
    }
}
