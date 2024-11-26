using Godot;


public partial class GameManager : Node2D
{



	private Vector2 playerPosition = new Vector2 (0, 0);

	// RESPAWN POINT

		public Marker2D RespawnPoint;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RespawnPlayer(){

		PlayerController pc = GetNode<PlayerController>("Player");
		RespawnPoint = GetNode<Marker2D>("RespawnPoint");
		pc.GlobalPosition = RespawnPoint.GlobalPosition;
		pc.RespawnPlayer();
	}	

	private void _on_player_death(){
		RespawnPlayer();
	}


	private void _on_player_inventory(){

	}

	private void _on_player_pause(){
		GetTree().Paused = true;
		playerPosition = GetNode<PlayerController>("Player").GlobalPosition;
		GetTree().ChangeSceneToFile("scenes/pauseMenu.tscn");

	}


	private void _on_player_resume(){
		
		

	}
}
