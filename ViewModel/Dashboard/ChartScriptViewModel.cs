namespace MedManager.ViewModel.Dashboard;

public class ChartScriptViewModel
{
    public List<string> Labels { get; set; } = new();
    public List<int> Data { get; set; } = new();
    
    public string ChartName { get; set; }
    public string ChartType { get; set; }
    
    public string Label { get; set; }
    
    public string[] BackgroundColor { get; set; }
    public string[] BorderColor { get; set; }
}