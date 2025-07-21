using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.DTO
{
    public class DTO_Autor
    {
        [Key]
        public required int LivroId { get; set; }
        [Display(Name = "Autor")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required int AutorId { get; set; }
        [Display(Name = "Ordem do Autor")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required int OrdemAutoria { get; set; }
        public required SelectList Autores { get; set; }
    }
}
