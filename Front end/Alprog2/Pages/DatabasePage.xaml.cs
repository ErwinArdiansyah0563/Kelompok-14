using System.Collections.ObjectModel;
using Alprog2.Models;
using Alprog2.Services;

namespace Alprog2.Pages;

public partial class DatabasePage : ContentPage
{
    private readonly MongoService _mongoService = new();
    public ObservableCollection<TemperatureModel> TemperatureList { get; set; } = new();
    private const double BatasSuhu = 35.0;
    private bool _isRunning = false;

    public DatabasePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //// Cek apakah COM port sudah dipilih
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
            await LoadData();
            await Task.Delay(5000); 
        }
    }

    private async Task LoadData()
    {
        try
        {
            var data = await _mongoService.GetAllDataAsync();
            TemperatureList.Clear();

            foreach (var item in data)
            {
                TemperatureList.Add(item);

                // Peringatan suhu tinggi
                if (item.suhu >= BatasSuhu)
                {
                    await DisplayAlert("Peringatan", $"Suhu tinggi terdeteksi: {item.suhu} °C pada {item.Waktu:g}", "OK");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is TemperatureModel item)
        {
            await _mongoService.DeleteDataAsync(item.Id);
            await LoadData();
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is TemperatureModel item)
        {
            string newValueStr = await DisplayPromptAsync("Edit Data", "Masukkan nilai baru:", initialValue: item.suhu.ToString());
            if (double.TryParse(newValueStr, out double newValue))
            {
                item.suhu = newValue;
                await _mongoService.UpdateDataAsync(item);
                await LoadData();
            }
        }
    }
}
