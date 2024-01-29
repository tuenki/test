using System;
using System.Collections.Generic;

namespace Usuarios.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Usuario1 { get; set; }

    public string? Password { get; set; }
}
