using Godot;
using System;

public partial class WinScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void _on_main_menu_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}


	private void _on_restart_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}
	private void _on_quit_pressed(){
		GetTree().Quit();
	}
}	
