using System;
using System.IO.Ports;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Alprog2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            LoadComPorts(); // Load daftar COM saat halaman dimuat
        }

        // Fungsi untuk menampilkan daftar COM port
        private void LoadComPorts()
        {
            var ports = SerialPort.GetPortNames();
            ComPortPicker.ItemsSource = ports.ToList();
        }

        // Fungsi saat COM port dipilih
        private void ComPortPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComPortPicker.SelectedIndex >= 0)
            {
                StartButton.IsVisible = true; // Tampilkan tombol Start jika COM dipilih
            }
            else
            {
                StartButton.IsVisible = false;
            }
        }

        // Fungsi saat tombol Start ditekan
        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            string selectedPort = ComPortPicker.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedPort))
            {
                await DisplayAlert("Info", $"Menghubungkan ke {selectedPort}", "OK");
                // Tambahkan logika koneksi ke SerialPort di sini jika dibutuhkan
            }
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
