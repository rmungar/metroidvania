using Godot;


public partial class GameManager : Node2D
{

	bool gamePaused = false;

	private Vector2 playerPosition = new Vector2 (0, 0);

	// RESPAWN POINT

		public Node2D RespawnPoint;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	
	}

	public void RespawnPlayer(){
		RespawnPoint = GetNode<Node2D>("RespawnPoint");
		PlayerController pc = GetNode<PlayerController>("Player");
		
		pc.GlobalPosition = RespawnPoint.Position;
		pc.RespawnPlayer();
	
	}	

	private void _on_player_death(){
		RespawnPlayer();
	}



	private void _on_player_pause(){

		CharacterBody2D player = GetNode<CharacterBody2D>("Player");
		PlayerController pc = player as PlayerController;
		Camera2D camera = pc.GetNode<Camera2D>("Camera2D");
		CanvasLayer cl = camera.GetNode<CanvasLayer>("CanvasLayer");
		PauseMenu pm = cl.GetNode<PauseMenu>("PauseMenu");
		pm.Show();
		gamePaused = true;
		pc.isPaused = true;

	}


	private void _on_player_resume(){
		CharacterBody2D player = GetNode<CharacterBody2D>("Player");
		PlayerController pc = player as PlayerController;
		Camera2D cam = player.GetNode<Camera2D>("Camera2D");
		CanvasLayer cl = cam.GetNode<CanvasLayer>("CanvasLayer");
		PauseMenu pm = cl.GetNode<PauseMenu>("PauseMenu");
		pm.Hide();
		gamePaused = false;
		pc.isPaused = false;

	}


	private void updateRespawnPoint(float PositionX, float PositionY){
		RespawnPoint = GetNode<Node2D>("RespawnPoint");
		RespawnPoint.Position = new Vector2 (PositionX, PositionY);
	}


	private void _on_check_points_first_check_point(float PositionX, float PositionY){
		updateRespawnPoint(PositionX, PositionY);
	}

	private void _on_check_points_second_check_point(float PositionX, float PositionY){
		updateRespawnPoint(PositionX, PositionY);
	}

	private void _on_check_points_third_check_point(float PositionX, float PositionY){
		updateRespawnPoint(PositionX, PositionY);
	}

	private void _on_check_points_fourth_check_point(float PositionX, float PositionY){
		updateRespawnPoint(PositionX, PositionY);
	}


	private void _on_kill_zone_body_entered(Node2D body){

		if (body is CharacterBody2D){
			if (body is PlayerController){
				RespawnPlayer();
			}
		}

	}


	private void _on_win(){
		GetTree().ChangeSceneToFile("res://scenes/win_Screen.tscn");
	}


}
