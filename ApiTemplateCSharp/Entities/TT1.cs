using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTemplateCSharp.Entities
{
    [Table("t_t1")]
    public class TT1
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
        [Column("c2")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "C2")]
        public string C2 { get; set; }
        [Column("c3")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "C3")]
        public string C3 { get; set; }
        [Column("c4")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "C4")]
        public string C4 { get; set; }
        [Column("date_updated")]
        [Display(Name = "DateUpdated")]
        public string DateUpdated { get; set; }
    }
}
