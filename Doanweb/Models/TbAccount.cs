using System;
using System.Collections.Generic;

namespace Doanweb.Models;

public partial class TbAccount
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? IsActive { get; set; }

    public virtual TbRole? Role { get; set; }

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();

    public virtual ICollection<TbRental> TbRentals { get; set; } = new List<TbRental>();
}
