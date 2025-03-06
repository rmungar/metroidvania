using System.Collections;
using Godot;


public partial class GameManager : Node2D
{

	bool gamePaused = false;

	public int PlayerDeaths = 0;

	public int LastCheckpoint = 0;





	private Vector2 playerPosition = new Vector2 (0, 0);

	// RESPAWN POINT

	public Node2D RespawnPoint;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node child in GetTree().Root.GetChildren())
		{
			if (child.Name == "Game"){
				if (PlayerDeaths != 0 || LastCheckpoint != 0){
					setDeaths(PlayerDeaths);
					setCheckPoint(LastCheckpoint);
				}
			}
		}
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
		CharacterBody2D player = GetNode<CharacterBody2D>("Player");
		PlayerController pc = player as PlayerController;
		if(pc.checkPoint < 4){
			pc.increaseCp();
		}
		RespawnPoint = GetNode<Node2D>("/root/Game/RespawnPoint");
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

	
	public void setDeaths(int deaths){
		CharacterBody2D player = GetNode<CharacterBody2D>("Player");
		PlayerController pc = player as PlayerController;
		Camera2D cam = pc.GetNode<Camera2D>("Camera2D");
		Label label = cam.GetNode<Label>("Deaths");
		label.Text = $"Deaths: {deaths}";
	}

	public void setCheckPoint(int checkPoint){
		if (checkPoint == 1){
			_on_check_points_first_check_point(479.0f, 304.0f);
		}
		if (checkPoint == 2){
			_on_check_points_second_check_point(1546.0f, 208.0f);
		}
		if (checkPoint == 3){
			_on_check_points_third_check_point(2214.0f, 1088.0f);
		}
		if (checkPoint == 4){
			_on_check_points_fourth_check_point(2844.0f, 369.0f);
		}
	}

}
