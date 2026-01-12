using System;
using System.Collections.Generic;

namespace Doanweb.Models;

public partial class TbRentalDetail
{
    public int RentalDetailId { get; set; }

    public int RentalId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public int PricePerDay { get; set; }

    public int Days { get; set; }

    public int? SubTotal { get; set; }

    public string? ConditionOnRent { get; set; }

    public string? ConditionOnReturn { get; set; }

    public bool? IsReturned { get; set; }

    public virtual TbProduct Product { get; set; } = null!;

    public virtual TbRental Rental { get; set; } = null!;
}
