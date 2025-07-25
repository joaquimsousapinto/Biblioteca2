using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.Metadata
{
    public class MetadataEmprestimo
    {
        [Display(Name = "N.º")]
        public int Id { get; set; }
        [Display(Name = "Utilizador")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public int UtilizadorId { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public int EstadoId { get; set; }

        [Display(Name = "Data de empréstimo")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public DateOnly DataEmprestimo { get; set; }

        [Display(Name = "Devolução prevista")]
        [DataType(DataType.Date)]
        public DateOnly? DataDevolucaoPrevista { get; set; }

        [Display(Name = "Devolução devolução")]
        [DataType(DataType.Date)]
        public DateOnly? DataDevolucaoReal { get; set; }

        [Display(Name = "Livro(s)")]
        public virtual ICollection<EmprestimoLivro> EmprestimoLivros { get; set; } = new List<EmprestimoLivro>();

        [Display(Name = "Estado(s)")]
        public required virtual EmprestimoEstado Estado { get; set; }

        [Display(Name = "Utilizador(es)")]
        public required virtual Utilizador Utilizador { get; set; }
    }
}
