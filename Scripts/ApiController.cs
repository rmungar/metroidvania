using Godot;
using System;
using System.Text;
using System.Text.Json;

public partial class ApiController : Node
{
    private HttpRequest requester;

    private int deaths = 0;
    private int lastCp = 0;

    public override void _Ready()
    {
        requester = GetNode<HttpRequest>("HTTPRequest");
        requester.RequestCompleted += OnRequestCompleted;
    }

    public void _on_resume_pressed()
    {
        // Hacer la petición GET al servidor para verificar si el jugador ya existe
        string[] headers = { "Content-Type: application/json" };
        Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Get/1", headers, HttpClient.Method.Get);
        
        if (error != Error.Ok)
        {
            GD.PrintErr("Error al hacer la solicitud GET: " + error);
        }
    }

    public void _on_player_save(int Deaths, int LastCheckpoint)
    {
        string[] headers = { "Content-Type: application/json" };
        Player player = new Player(1, LastCheckpoint, Deaths); // Corrige la posición de LastCheckpoint y Deaths

        this.deaths = Deaths;
        this.lastCp = LastCheckpoint;

        // Convertir el objeto Player a JSON
        string jsonData = player.ToJsonContent();

        // Primero, realizamos una solicitud GET para comprobar si el jugador existe
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

        if (responseCode == 200)
        {
            // Si el jugador existe (respuesta 200), actualizarlo
            if (responseText != "")
            {
                // El jugador ya existe, así que realizar una solicitud PUT para actualizar
                GD.Print("El jugador existe, actualizando...");
                UpdatePlayer(lastCp, deaths);
            }
        }
        else if (responseCode == 404)
        {
            // Si el jugador no existe (respuesta 404), crear uno nuevo
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
        // Crear un nuevo jugador
        Player player = new Player(1, lastCheckPoint, deaths); // Ajusta las variables si es necesario
        string jsonData = player.ToJsonContent();

        // Enviar la solicitud POST para crear un jugador
        string[] headers = { "Content-Type: application/json" };
        Error error = requester.Request("https://nthapiweb.onrender.com/api/Player/Create", headers, HttpClient.Method.Post, jsonData);
        
        if (error != Error.Ok)
        {
            GD.PrintErr("Error al hacer la solicitud POST para crear el jugador: " + error);
        }
    }

    private void UpdatePlayer(int deaths, int lastCheckPoint)
    {
        // Parsear el JSON existente para obtener los datos actuales del jugador
        try
        {
        
            // Crear los datos actualizados
            Player player = new Player(1, lastCheckPoint, deaths); // Asegúrate de actualizar los valores de Deaths y LastCheckpoint
            string jsonData = player.ToJsonContent();

            // Realizar la solicitud PUT para actualizar los datos
            string[] headers = { "Content-Type: application/json" };
            Error error = requester.Request($"https://nthapiweb.onrender.com/api/Player/Update/1", headers, HttpClient.Method.Put, jsonData);
            
            if (error != Error.Ok)
            {
                GD.PrintErr("Error al hacer la solicitud PUT para actualizar el jugador: " + error);
            }
        }
        catch (JsonException e)
        {
            GD.PrintErr("Error al parsear el JSON de la respuesta: " + e.Message);
        }
    }
}
