using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace templateMariaDb.Models;

public class Estado
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    [JsonIgnore]
    public List<Cidade> Cidades { get; set; } = new List<Cidade>();
}
