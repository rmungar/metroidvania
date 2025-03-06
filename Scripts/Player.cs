using System.Net.Http.Json;

public class Player
{
    public int Id { get; set; }
    public int LastCheckpoint { get; set; }
    public int Deaths { get; set; }

    public Player(int id, int deaths, int lastCheckpoint)
    {
        Id = id;
        this.LastCheckpoint = lastCheckpoint;
        this.Deaths = deaths;
    }

    // Método que devuelve el contenido JSON necesario para la solicitud
    public string ToJsonContent()
    {
        // Creamos un objeto que contiene el jugador
        var playerData = new
        {
            player = new
            {
                Id,
                LastCheckpoint,
                Deaths
            }
        };

        // Usamos JsonContent para serializar el objeto
        return System.Text.Json.JsonSerializer.Serialize(playerData);
    }
}
