using System;
using System.Collections.Generic;

namespace Traveler.DALayer.Models;

public partial class CustomerCare
{
    public int QueryId { get; set; }

    public int? BookingId { get; set; }

    public string? Query { get; set; }

    public string? QueryStatus { get; set; }

    public int? Assignee { get; set; }

    public string? QueryAnswer { get; set; }

    public virtual Employee? AssigneeNavigation { get; set; }

    public virtual BookPackage? Booking { get; set; }
}
