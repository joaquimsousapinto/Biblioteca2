﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Biblioteca2.Models;

public partial class LivroAutor
{
    public int LivroId { get; set; }

    public int AutorId { get; set; }

    public int OrdemAutoria { get; set; }

    public virtual Autor Autor { get; set; }

    public virtual Livro Livro { get; set; }
}