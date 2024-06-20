using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class AttendEvent
{
    [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    [ForeignKey("Event")]
    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public DateTime? AttendedDate { get; set; }

}
