using System;
using System.Collections.Generic;
using System.Data;

namespace Entities.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    private string? _role;

    public string? Role
    {
        get => _role;

        set
        {
            _role = (value == "host" || value == "guest") ? value : "guest";
        }
    }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<AttendEvent> AttendEvents { get; set; } = new List<AttendEvent>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
