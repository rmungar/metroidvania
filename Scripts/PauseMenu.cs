using Godot;
using System;


public partial class PauseMenu : Control
{
	bool isPaused = false;


	

	public override void _Ready(){
		GetTree().Paused = false;
		Visible = false;
	}


	public override void _Process(double delta)
	{	
		CanvasLayer cl = GetParent<CanvasLayer>();
		Camera2D cam = cl.GetParent<Camera2D>();
		PlayerController pc = cam.GetParent<PlayerController>();
		GlobalPosition = pc.GlobalPosition + new Vector2(-192,-178);
	}


	public void _unhandledInput (InputEvent @event){
		if (isPaused){
			if (@event is InputEventKey eventKey){
				if(eventKey.IsActionPressed("pause")){
					isPaused = false;
					GetTree().Paused = false;
					Visible = false;
				}
			}
		}
	}


	public void _on_resume_pressed(){
		isPaused = false;
		GetTree().Paused = false;
		Visible = false;
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}


	public void _on_save_pressed(){
		// Save the game
	}

	public void onPause(){
		isPaused = true;
		GetTree().Paused = true;
		Visible = true;
	}


}
