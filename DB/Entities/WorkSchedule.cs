using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class WorkSchedule
{
    public int ScheduleId { get; set; }

    public string WorkDate { get; set; } = null!;

    public string WorkTime { get; set; } = null!;
}
