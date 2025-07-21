using Biblioteca2.Models.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca2.Models
{
    [ModelMetadataType(typeof(MetadataAutor))]
    public partial class Autor
    { }

    [ModelMetadataType(typeof(MetadataLivro))]
    public partial class Livro
    { }

    [ModelMetadataType(typeof(MetadataUtilizador))]
    public partial class Utilizador
    { }
}
