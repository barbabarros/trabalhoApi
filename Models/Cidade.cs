using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace templateMariaDb.Models;

public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int EstadoId { get; set; }
    public Estado? Estado { get; set; }
    [JsonIgnore]
    public List<LinhaOnibus> LinhasOnibus { get; set; } = new List<LinhaOnibus>();

}
