using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.DTO
{
    public class DTO_Livro
    {

        [Display(Name = "Empréstimo")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required int EmprestimoId { get; set; }

        [Display(Name = "Livro")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required int LivroId { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required int Quantidade { get; set; }
        public SelectList Livros { get; set; }
    }
}
