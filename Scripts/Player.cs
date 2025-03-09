using System.Collections.Generic;
using System.Net.Http.Json;

public class Player
{
    public string? _id { get; set; }
    public int lastCheckPoint { get; set; }
    public int deaths { get; set; }

    public Player(string? id, int deaths, int lastCheckpoint)
    {
        this._id = id;
        this.lastCheckPoint = lastCheckpoint;
        this.deaths = deaths;
    }

    // Método que devuelve el contenido JSON necesario para la solicitud
    public string ToJsonContent()
    {
        // Creamos un objeto que contiene el jugador
        var playerData = new Dictionary<string, object>
        {
            { "_id", this._id },
            { "lastCheckPoint", this.lastCheckPoint },
            { "deaths", this.deaths }
        };

        // Usamos JsonContent para serializar el objeto
        return System.Text.Json.JsonSerializer.Serialize(playerData);
    }
}
