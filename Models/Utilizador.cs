﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Biblioteca2.Models;

public partial class Utilizador
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Morada { get; set; }

    public string Telefone { get; set; }

    public string Email { get; set; }

    public DateOnly? DataNascimento { get; set; }

    public string Sexo { get; set; }

    public virtual ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}