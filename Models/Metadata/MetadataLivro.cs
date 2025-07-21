using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.Metadata
{
    public class MetadataLivro
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [StringLength(256, MinimumLength = 4, ErrorMessage = "O campo {0} deve ter entre {2} e {1} carateres.")]
        public required string Titulo { get; set; }
        [Display(Name = "Ano de publicação")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public int AnoPublicacao { get; set; }

        [Display(Name = "Editora")]
        public int EditoraId { get; set; }

        [Display(Name = "Ano de Edição")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public int AnoEdicao { get; set; }

        [Display(Name = "N.º de Exemplares")]
        public int? Exemplares { get; set; }

        [Display(Name = "Editora(s)")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public virtual required Editora Editora { get; set; }

        [Display(Name = "Empréstimo(s)")]
        public virtual ICollection<EmprestimoLivro> EmprestimoLivros { get; set; } = new List<EmprestimoLivro>();

        [Display(Name = "Autor(es)")]
        public virtual ICollection<LivroAutor> LivroAutors { get; set; } = new List<LivroAutor>();

        [Display(Name = "Categoria(s)")]
        public virtual ICollection<Categorium> Categoria { get; set; } = new List<Categorium>();
    }
}
