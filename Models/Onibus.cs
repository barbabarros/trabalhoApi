using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace templateMariaDb.Models;

public class Onibus
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int LinhaOnibusId { get; set; }

    public LinhaOnibus? LinhaOnibus { get; set; }
}
