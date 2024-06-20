using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public DateTime DateTime { get; set; }

    public string? Category { get; set; }

    public string? Description { get; set; }

    public string? Picture { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<AttendEvent> AttendEvents { get; set; } = new List<AttendEvent>();

    public virtual User User { get; set; } = null!;
}
