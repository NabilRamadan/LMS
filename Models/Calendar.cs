using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRUDApi.Models
{
    [Table("Calendar")]

    public partial class Calendar
    {

        [Key]
        [Column("calendar_ID")]
        [StringLength(50)]
        [Unicode(false)]
        public string CalendarId { get; set; } = null!;

        [ForeignKey("UserId")]
        [Column("user_ID")]
        [StringLength(50)]
        [Unicode(false)]
        public string? UserId { get; set; }
        [Column("start_date")]

        public DateTime StartDate { get; set; }
        [Column("end_date")]

        public DateTime EndDate { get; set; }
        [Column("body")]
        [StringLength(255)]
        public string Body { get; set; } = null!;

        public virtual User? User { get; set; }

    }
}
