using System.ComponentModel.DataAnnotations;

namespace Biblioteca2.Models.Metadata
{
    public class MetadataAutor
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [StringLength(256, MinimumLength = 4, ErrorMessage = "O campo {0} deve ter entre {2} e {1} carateres.")]
        public required string Nome { get; set; }
        [StringLength(1024, MinimumLength = 4, ErrorMessage = "O campo {0} deve ter no máximo {1} carateres.")]
        public required string Bibliografia { get; set; }
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? DataNascimento { get; set; }
        [Display(Name = "Data de Morte")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? DataMorte { get; set; }
        [Display(Name = "Livro(s)")]
        public virtual ICollection<LivroAutor> LivroAutors { get; set; } = new List<LivroAutor>();

    }
}
