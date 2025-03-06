using System.Net.Http.Json;

public class Player
{
    public int id { get; set; }
    public int lastCheckPoint { get; set; }
    public int deaths { get; set; }

    public Player(int id, int deaths, int lastCheckpoint)
    {
        this.id = id;
        this.lastCheckPoint = lastCheckpoint;
        this.deaths = deaths;
    }

    // Método que devuelve el contenido JSON necesario para la solicitud
    public string ToJsonContent()
    {
        // Creamos un objeto que contiene el jugador
        var playerData = new
        {
            player = new
            {
                this.id,
                this.lastCheckPoint,
                this.deaths
            }
        };

        // Usamos JsonContent para serializar el objeto
        return System.Text.Json.JsonSerializer.Serialize(playerData);
    }
}
