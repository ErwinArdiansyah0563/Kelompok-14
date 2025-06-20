using Alprog2.Services;

namespace Alprog2.Pages;

public partial class DataNumerikPage : ContentPage
{
    private readonly MongoService _mongoService = new();
    private bool _isRunning = false;

    public DataNumerikPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Cek apakah COM port sudah dipilih
        if (string.IsNullOrEmpty(AppState.SelectedComPort))
        {
            await DisplayAlert("Info", "Silakan pilih COM port terlebih dahulu.", "OK");
            await Shell.Current.GoToAsync("//ComPortPage");
            return;
        }

        _isRunning = true;
        StartAutoRefresh();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isRunning = false;
    }

    private async void StartAutoRefresh()
    {
        while (_isRunning)
        {
            await LoadData();
            await Task.Delay(5000); // Refresh setiap 5 detik
        }
    }

    private async Task LoadData()
    {
        try
        {
            var data = await _mongoService.GetLatestDataAsync();
            LabelTemperature.Text = data != null ? $"{data.suhu:F5} °C" : "No Data";
            LabelResistance.Text = $"{data.resistance} Ω";
            LabelNumerik.Text = $"{data.numerik:F5} °C";
        }
        catch (Exception ex)
        {
            LabelTemperature.Text = $"Error: {ex.Message}";
            LabelResistance.Text = $"Error: {ex.Message}";
            LabelNumerik.Text = $"Error: {ex.Message}";
        }
    }
}
