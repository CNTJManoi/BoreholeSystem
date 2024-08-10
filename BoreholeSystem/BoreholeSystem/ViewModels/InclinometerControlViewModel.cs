using Avalonia.Media;
using Avalonia.Threading;
using BoreholeSystem.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace BoreholeSystem.ViewModels
{
    public partial class InclinometerControlViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly Timer _timer;
        private SerialPort _serialPort;
        private double[] _yValues = new double[5];
        private double[] _zValues = new double[5];
        private double[] _xValues = new double[5];
        private int _valueIndex = 0;
        private Quaternion quanterion;
        private int _valuesReceived = 0;

        public InclinometerControlViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            // Инициализация доступных портов
            AvailablePorts = new ObservableCollection<string>();
            OxisY = "0";
            errorMessage = "";
            OxisX = "0";
            OxisZ = "0";
            // Установка начального статуса
            UpdateDeviceStatus(false);

            _timer = new Timer(1000); // 1000 мс = 1 секунда
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
            UpdateAvailablePorts();
        }
        public InclinometerControlViewModel() { }

        [ObservableProperty]
        private string errorMessage;

        // Список доступных портов
        [ObservableProperty]
        private ObservableCollection<string> availablePorts;

        // Выбранный порт
        [ObservableProperty]
        private string selectedPort;
        private string selectedPortWithWork;

        // Статус устройства (онлайн/оффлайн)
        [ObservableProperty]
        private string deviceStatus;

        // Цвет индикатора статуса
        [ObservableProperty]
        private SolidColorBrush deviceStatusColor;

        [ObservableProperty]
        private string oxisY;

        [ObservableProperty]
        private string oxisX;

        [ObservableProperty]
        private string oxisZ;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isError;

        // Команда для подтверждения выбора
        [RelayCommand]
        private void Confirm()
        {
            selectedPortWithWork = selectedPort;
            if (availablePorts.Contains(selectedPortWithWork))
            {
                UpdateDeviceStatus(true);
                
            }
            else
            {
                UpdateDeviceStatus(false);
                selectedPortWithWork = null;
            }
        }

        // Метод обновления статуса устройства
        private void UpdateDeviceStatus(bool isOnline)
        {
            DeviceStatus = isOnline ? "Online" : "Offline";
            DeviceStatusColor = new SolidColorBrush(isOnline ? Colors.Green : Colors.Red);
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Обновляем доступные порты
            UpdateAvailablePorts();
        }

        private void UpdateAvailablePorts()
        {
            // Получаем список доступных COM портов
            AvailablePorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            if (!string.IsNullOrEmpty(selectedPortWithWork))
            {
                if (availablePorts.Contains(selectedPortWithWork))
                {
                    Dispatcher.UIThread.InvokeAsync(() => UpdateDeviceStatus(true));

                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() => UpdateDeviceStatus(false));

                }
            }
        }

        [RelayCommand]
        private void Work()
        {
            if (!IsLoading)
            {
                if (!string.IsNullOrEmpty(selectedPort))
                {
                    try
                    {
                        _valueIndex = 0;
                        _valuesReceived = 0;
                        _serialPort = new SerialPort(selectedPort, 115200);
                        _serialPort.DataReceived += OnDataReceived;
                        _serialPort.Open();
                        IsLoading = true;
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = "Возникла ошибка при подключении к плате: " + e.Message;
                        IsError = true;
                    }
                }
            }
        }
        [RelayCommand]
        private void ConfirmError()
        {
            IsError = false;
        }
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadLine();

            // Разделение строки на части
            var parts = data.Split(',');

            if (parts.Length >= 3)
            {
                // Преобразование значений и сохранение в массивы
                if (double.TryParse(parts[0].Replace('.', ','), out double zValue))
                {
                    _zValues[_valueIndex] = zValue;
                }
                if (double.TryParse(parts[1].Replace('.',','), out double yValue))
                {
                    _yValues[_valueIndex] = yValue;
                }
                if (double.TryParse(parts[2].Replace('.', ','), out double xValue))
                {
                    _xValues[_valueIndex] = xValue;
                }

                _valueIndex++;
                _valuesReceived++;

                if (_valuesReceived >= 5)
                {
                    // Вычисление среднего значения
                    OxisY = _yValues.Average().ToString("F2");
                    OxisX = _xValues.Average().ToString("F2");
                    OxisZ = _zValues.Average().ToString("F2");
                    GetQuant();
                    StopReading();
                    IsLoading = false;
                }
            }
        }

        private void StopReading()
        {
            _serialPort.Close();
        }
        private void GetQuant()
        {
            _serialPort.WriteLine("quant");
            Task.Delay(1);
            string data = _serialPort.ReadLine();
            var parts = data.Split(',');
            if (parts.Length >= 4)
            {
                if (float.TryParse(parts[0].Replace('.', ','), out float w)
                    && float.TryParse(parts[1].Replace('.', ','), out float x)
                    && float.TryParse(parts[2].Replace('.', ','), out float y)
                    && float.TryParse(parts[3].Replace('.', ','), out float z))
                {
                    quanterion = new Quaternion(x, y, z, w);
                }
            }
            else GetQuant();
        }
    }
}
