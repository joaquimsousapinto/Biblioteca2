using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.DTO
{
    public class DTO_Categoria
    {
        public required int LivroId { get; set; }
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required int CategoriaId { get; set; }
        public required SelectList Categorias { get; set; }
    }
}
