using AP2_AspNetMvc.Models;

namespace AP2_AspNetMvc.ViewModel.Patient;

public class AddPatientViewModel
{
    public Models.Patient Patient { get; set; }
    public List<Doctor> Doctors { get; set; }
}