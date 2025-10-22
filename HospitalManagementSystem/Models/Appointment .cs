using HospitalManagementSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    [ForeignKey("PatientId")]
    public int PatientId { get; set; }
    public virtual Patient Patient { get; set; }

    [ForeignKey("DoctorId")]
    public int? DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }

    [ForeignKey("RoomId")]
    public int? RoomId { get; set; }
    public virtual ClinicRoom ClinicRoom { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public TimeSpan AppointmentTime { get; set; } // 08:00, 09:00...

    [StringLength(20)]
    public string Status { get; set; } // Chờ khám, Đang khám, Hoàn thành, Hủy

    [StringLength(500)]
    public string Notes { get; set; }

    public DateTime? CheckInTime { get; set; } // Thời gian check-in
    public DateTime? CheckOutTime { get; set; } // Thời gian hoàn thành
}