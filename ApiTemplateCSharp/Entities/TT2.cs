using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTemplateCSharp.Entities
{
    [Table("t_t2")]
    public class TT2
    {
        [Column("id")]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Column("c1")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "C1")]
        public string C1 { get; set; }
        [Column("d1")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "D1")]
        public string D1 { get; set; }
        [Column("d2")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "D2")]
        public string D2 { get; set; }
        [Column("d3")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "D3")]
        public string D3 { get; set; }
        [Column("date_updated")]
        [Display(Name = "DateUpdated")]
        public string DateUpdated { get; set; }
    }
}
