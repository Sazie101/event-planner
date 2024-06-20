﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class EventDTO
    {
        public string Name { get; set; } = null!;

        public string? Location { get; set; }

        public DateTime DateTime { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public string? Picture { get; set; }
    }
}