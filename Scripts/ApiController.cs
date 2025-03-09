using Godot;
using System;
using System.Text;
using System.Text.Json;

public partial class ApiController : Node
{
	private HttpRequest requester;

	private int deaths = 0;
	private int lastCp = 0;
	private bool isSaveRequest = false;
	private bool isCheckingExistence = false;

	public override void _Ready()
	{
		requester = GetNode<HttpRequest>("HTTPRequest");
		requester.RequestCompleted += OnRequestCompleted;
	}

	// Método llamado cuando el usuario quiere reanudar la partida
	public void _on_resume_pressed()
	{
		// Solo comprobar si hay datos en la base de datos
		isSaveRequest = false;
		string[] headers = { "Content-Type: application/json" };
		Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Get/1", headers, HttpClient.Method.Get);

		if (error != Error.Ok)
		{
			GD.PrintErr("Error al hacer la solicitud GET: " + error);
		}
	}

	// Método llamado cuando el jugador guarda la partida
	public void _on_player_save(int Deaths, int LastCheckpoint)
	{
		isSaveRequest = true;
		isCheckingExistence = true; // Indica que estamos verificando si el jugador existe antes de guardar
		deaths = Deaths;
		lastCp = LastCheckpoint;

		string[] headers = { "Content-Type: application/json" };
		Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Get/1", headers, HttpClient.Method.Get);

		if (error != Error.Ok)
		{
			GD.PrintErr("Error al hacer la solicitud GET para verificar existencia: " + error);
		}
	}

	// Manejo de respuestas
	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		string responseText = Encoding.UTF8.GetString(body);
		GD.Print($"Código de respuesta: {responseCode}");
		GD.Print($"Cuerpo de la respuesta: {responseText}");

		if (!isSaveRequest) // Si la solicitud es para reanudar partida
		{
			if (responseCode == 200)
			{
				GD.Print("Datos de jugador encontrados.");
				try
				{
					JsonDocument json = JsonDocument.Parse(responseText);
					string mongoId = json.RootElement.GetProperty("_id").GetString();
					lastCp = json.RootElement.GetProperty("lastCheckPoint").GetInt32();
					deaths = json.RootElement.GetProperty("deaths").GetInt32();

					GD.Print($"Datos de jugador - Muertes: {deaths}, Último checkpoint: {lastCp}");

					// Aplicar los datos al jugador
					ApplyPlayerData(deaths, lastCp);
					GetTree().ChangeSceneToFile("res://scenes/game.tscn");
				}
				catch (Exception e)
				{
					GD.PrintErr("Error al extraer los datos de MongoDB: " + e.Message);
				}
			}
			else
			{
				GD.Print("No se encontraron datos de jugador.");
			}
			return;
		}

		// Si la solicitud era para guardar la partida
		if (isCheckingExistence)
		{
			isCheckingExistence = false; // Evita que se vuelva a hacer otro GET
			if (responseCode == 200)
			{
				// Jugador ya existe, actualizarlo
				try
				{
					JsonDocument json = JsonDocument.Parse(responseText);
					string mongoId = json.RootElement.GetProperty("_id").GetString();
					GD.Print("MongoDB ID obtenido: " + mongoId);
					UpdatePlayer(mongoId, lastCp, deaths);
				}
				catch (Exception e)
				{
					GD.PrintErr("Error al extraer el ID de MongoDB: " + e.Message);
				}
			}
			else if (responseCode == 404)
			{
				// Jugador no existe, crearlo
				GD.Print("El jugador no existe, creando uno nuevo...");
				CreatePlayer();
			}
			else
			{
				GD.PrintErr($"Error en la respuesta del servidor: Código {responseCode}\n {responseText}");
			}
		}
	}

	// Crear un nuevo jugador si no existe (solo cuando el jugador guarda la partida)
	private void CreatePlayer()
	{
		PlayerController pc = GetParent().GetNode<PlayerController>("Player");

		pc.PrintData();
		GD.Print(pc.checkPoint);
		GD.Print(pc.deaths);

		// No establecer ID, MongoDB lo generará automáticamente
		Player player = new Player("1", pc.deaths, pc.checkPoint); // ID se genera automáticamente en MongoDB
		string jsonData = player.ToJsonContent();
		string[] headers = { "Content-Type: application/json" };

		Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Create", headers, HttpClient.Method.Post, jsonData);

		if (error != Error.Ok)
		{
			GD.PrintErr("Error al hacer la solicitud POST para crear el jugador: " + error);
		}
	}

	// Actualizar los datos del jugador existente
	private void UpdatePlayer(string id, int deaths, int lastCheckPoint)
	{
		PlayerController pc = GetParent().GetNode<PlayerController>("Player");

		pc.PrintData();
		GD.Print(pc.checkPoint);
		GD.Print(pc.deaths);

		// Usar el _id real de MongoDB para actualizar los datos
		Player player = new Player("1", pc.deaths, pc.checkPoint);
		string jsonData = player.ToJsonContent();
		string[] headers = { "Content-Type: application/json" };

		string url = $"https://nthapiweb.onrender.com/api/Player/Update/{id}";
		Error error = requester.Request(url, headers, HttpClient.Method.Put, jsonData);

		if (error != Error.Ok)
		{
			GD.PrintErr("Error al hacer la solicitud PUT para actualizar el jugador: " + error);
		}
	}



	private void ApplyPlayerData(int deaths, int lastCp)
	{
		GameManager global = (GameManager)GetNode("/root/GameManager");
		global.setResumeInfo(deaths, lastCp);
	}


}
