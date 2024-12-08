using Godot;


public partial class GameManager : Node2D
{



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


	private void _on_player_inventory(){

	}

	private void _on_player_pause(){
		
		GetNode<Control>("PauseMenu").Show();

	}


	private void _on_player_resume(){
		
		GetNode<Control>("PauseMenu").Hide();
		PlayerController pc = GetNode<PlayerController>("Player");
		pc.isPaused = false;
	}


	private void updateRespawnPoint(float PositionX, float PositionY){
		RespawnPoint = GetNode<Node2D>("RespawnPoint");
		GD.Print("ACTUALIZANDO....");
		RespawnPoint.Position = new Vector2 (PositionX, PositionY);
		GD.Print("ACTUALIZADO");
	}


	private void _on_check_points_first_check_point(float PositionX, float PositionY){
		GD.Print("Actualizando a RP 1");
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
