using System;
using System.Collections.Generic;

namespace Doanweb.Models;

public partial class TbRentalStatus
{
    public int RentalStatusId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TbRental> TbRentals { get; set; } = new List<TbRental>();
}
