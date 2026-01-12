using System;
using System.Collections.Generic;

namespace Doanweb.Models;

public partial class TbRental
{
    public int RentalId { get; set; }

    public string RentalCode { get; set; } = null!;

    public int? AccountId { get; set; }

    public int? CustomerId { get; set; }

    public int? RentalStatusId { get; set; }

    public DateTime RentalStartDate { get; set; }

    public DateTime ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    public int? TotalDeposit { get; set; }

    public int? TotalRentalAmount { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsActive { get; set; }

    public virtual TbAccount? Account { get; set; }

    public virtual TbCustomer? Customer { get; set; }

    public virtual TbRentalStatus? RentalStatus { get; set; }

    public virtual ICollection<TbRentalDetail> TbRentalDetails { get; set; } = new List<TbRentalDetail>();
}
