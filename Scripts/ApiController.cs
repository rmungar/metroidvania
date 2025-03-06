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

    public override void _Ready()
    {
        requester = GetNode<HttpRequest>("HTTPRequest");
        requester.RequestCompleted += OnRequestCompleted;
    }

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

    public void _on_player_save(int Deaths, int LastCheckpoint)
    {
        isSaveRequest = true;
        deaths = Deaths;
        lastCp = LastCheckpoint;

        // Verificar si el jugador existe antes de guardarlo
        string[] headers = { "Content-Type: application/json" };
        Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Get/1", headers, HttpClient.Method.Get);
        
        if (error != Error.Ok)
        {
            GD.PrintErr("Error al hacer la solicitud GET para verificar existencia: " + error);
        }
    }

    private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        string responseText = Encoding.UTF8.GetString(body);
        GD.Print($"Código de respuesta: {responseCode}");
        GD.Print($"Cuerpo de la respuesta: {responseText}");

        if (!isSaveRequest)
        {
            // Si es una petición de Resume, solo imprimimos si hay datos o no
            if (responseCode == 200)
            {
                GD.Print("Datos de jugador encontrados.");
            }
            else
            {
                GD.Print("No se encontraron datos de jugador.");
            }
            return;
        }

        // Si es una petición de guardado
        if (responseCode == 200)
        {
            GD.Print("El jugador existe, actualizando...");
            UpdatePlayer(lastCp, deaths);
        }
        else if (responseCode == 404)
        {
            GD.Print("El jugador no existe, creando uno nuevo...");
            CreatePlayer(deaths, lastCp);
        }
        else
        {
            GD.PrintErr($"Error en la respuesta del servidor: Código {responseCode}");
        }
    }

    private void CreatePlayer(int deaths, int lastCheckPoint)
    {
        PlayerController pc = GetParent().GetNode<PlayerController>("Player");

        pc.PrintData();
        GD.Print(pc.checkPoint);
        GD.Print(pc.deaths);

        Player player = new Player(1, pc.checkPoint, pc.deaths);
        string jsonData = player.ToJsonContent();
        string[] headers = { "Content-Type: application/json" };
        //GD.Print("JSON Enviado: " + jsonData);

        Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Create", headers, HttpClient.Method.Post, jsonData);
        
        if (error != Error.Ok)
        {
            GD.PrintErr("Error al hacer la solicitud POST para crear el jugador: " + error);
        }
    }

    private void UpdatePlayer(int deaths, int lastCheckPoint)
    {

        PlayerController pc = GetParent().GetNode<PlayerController>("Player");

        pc.PrintData();
        GD.Print(pc.checkPoint);
        GD.Print(pc.deaths);

        Player player = new Player(1, pc.checkPoint, pc.deaths);
        string jsonData = player.ToJsonContent();
        string[] headers = { "Content-Type: application/json" };
        //GD.Print("JSON Enviado: " + jsonData);

        Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Update/1", headers, HttpClient.Method.Put, jsonData);
        
        if (error != Error.Ok)
        {
            GD.PrintErr("Error al hacer la solicitud PUT para actualizar el jugador: " + error);
        }
    }
}
