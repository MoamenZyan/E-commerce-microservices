using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ReviewerId { get; set; }
        public Guid ProductId { get; set; }
        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5")]
        public int Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
