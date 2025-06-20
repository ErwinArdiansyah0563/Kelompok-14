using Alprog2.Models;
using Alprog2.Services;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using Entry = Microcharts.ChartEntry;

namespace Alprog2.Pages;

public partial class GrafikPage : ContentPage
{
    private readonly MongoService _mongoService = new();
    private bool _isRunning = false;

    public GrafikPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (string.IsNullOrEmpty(AppState.SelectedComPort))
        {
            DisplayAlert("Info", "Silakan pilih COM port terlebih dahulu.", "OK");
            Shell.Current.GoToAsync("//ComPortPage");
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
            await LoadChart();
            await Task.Delay(5000); // Refresh setiap 5 detik
        }
    }

    private async Task LoadChart()
    {
        try
        {
            var data = await _mongoService.GetAllDataAsync();

            if (data == null || data.Count == 0)
            {
                ChartViewSensor.Chart = null;
                ChartViewNumerik.Chart = null;
                return;
            }

            var orderedData = data
                .Where(d => d.Waktu != null)
                .OrderBy(d => d.Waktu)
                .ToList();

            // Entries suhu asli
            var sensorEntries = orderedData.Select(d => new Entry((float)d.suhu)
            {
                Label = d.Waktu.ToString("HH:mm") ?? "-",
                ValueLabel = d.suhu.ToString("F1"),
                Color = SKColor.Parse("#3498db")
            }).ToList();

            // Entries suhu numerik
            var numerikEntries = orderedData.Select(d => new Entry((float)d.numerik)
            {
                Label = d.Waktu.ToString("HH:mm") ?? "-",
                ValueLabel = d.numerik.ToString("F1"),
                Color = SKColor.Parse("#e67e22")
            }).ToList();

            ChartViewSensor.Chart = new LineChart { Entries = sensorEntries, LineSize = 4, PointSize = 8 };
            ChartViewNumerik.Chart = new LineChart { Entries = numerikEntries, LineSize = 4, PointSize = 8 };
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


}
