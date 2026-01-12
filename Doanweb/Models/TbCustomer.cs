using System;
using System.Collections.Generic;

namespace Doanweb.Models;

public partial class TbCustomer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public int? IsRead { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Avatar { get; set; }

    public int? LocationId { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<TbRental> TbRentals { get; set; } = new List<TbRental>();
}
