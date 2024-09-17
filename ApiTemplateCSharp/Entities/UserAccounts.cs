using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTemplateCSharp.Entities
{
    [Table("user_accounts")]
    public class UserAccounts
    {
        [Column("id")]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Column("id_number")]
        [Required]
        [MaxLength(20)]
        [Display(Name = "IdNumber")]
        public string IdNumber { get; set; }
        [Column("full_name")]
        [Required]
        [MaxLength(50)]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
        [Column("username")]
        [Required]
        [MaxLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Column("password")]
        [MaxLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Column("section")]
        [Required]
        [MaxLength(50)]
        [Display(Name = "Section")]
        public string Section { get; set; }
        [Column("role")]
        [Required]
        [MaxLength(20)]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
