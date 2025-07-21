using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.Metadata
{
    public class MetadataUtilizador
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [StringLength(256, MinimumLength = 4, ErrorMessage = "O campo {0} deve ter entre {2} e {1} carateres.")]
        public required string Nome { get; set; }

        [StringLength(1024, ErrorMessage = "O campo {0} deve ter no máximo {1} carateres.")]
        public string? Morada { get; set; }

        public string? Telefone { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required string Sexo { get; set; }

        [Display(Name = "Empréstimo(s)")]
        public virtual ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}
