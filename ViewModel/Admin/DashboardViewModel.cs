using MedManager.Models;

namespace MedManager.ViewModel.Admin;

public class DashboardViewModel
{
    public List<Doctor> Doctors { get; set; } = new();
}