using System.IO.Ports;
using Alprog2.Services;
using Microsoft.Maui.Controls;

namespace Alprog2.Pages;

public partial class ComPortPage : ContentPage
{
    public ComPortPage()
    {
        InitializeComponent();
        LoadComPorts();
        StartButton.IsVisible = false;
    }

    private void LoadComPorts()
    {
        var ports = SerialPort.GetPortNames();
        ComPortPicker.ItemsSource = ports.ToList();
    }

    private void ComPortPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ComPortPicker.SelectedIndex >= 0)
        {
            AppState.SelectedComPort = ComPortPicker.SelectedItem.ToString(); // simpan global
            StartButton.IsVisible = true;
        }
        else
        {
            AppState.SelectedComPort = null;
            StartButton.IsVisible = false;
        }
    }

    private async void StartButton_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(AppState.SelectedComPort))
        {
            await DisplayAlert("Info", $"Memulai koneksi ke {AppState.SelectedComPort}", "OK");
            await Shell.Current.GoToAsync("//DataNumerikPage");
        }
    }

    private void OnRefreshPortsClicked(object sender, EventArgs e)
    {
        LoadComPorts();
    }
}
