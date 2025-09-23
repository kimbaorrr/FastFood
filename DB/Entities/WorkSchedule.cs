using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("work_schedule")]
[Index("WorkDate", Name = "uq__giolamvi__3272f2bc0b5c074d", IsUnique = true)]
public partial class WorkSchedule
{
    [Key]
    [Column("schedule_id")]
    public int ScheduleId { get; set; }

    [Column("work_date")]
    [StringLength(20)]
    public string WorkDate { get; set; } = null!;

    [Column("work_time")]
    [StringLength(50)]
    public string WorkTime { get; set; } = null!;
}
