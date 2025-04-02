using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace templateMariaDb.Models;

public class LinhaOnibus
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int CidadeId { get; set; }
    public Cidade? Cidade { get; set; }
    [JsonIgnore]
    public List<Onibus> Onibus { get; set; } = new List<Onibus>();

}
