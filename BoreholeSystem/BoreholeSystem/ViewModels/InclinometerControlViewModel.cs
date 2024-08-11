using Avalonia.Media;
using Avalonia.Threading;
using BoreholeSystem.Services;
using BoreholeSystem.Utils;
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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace BoreholeSystem.ViewModels
{
    public partial class InclinometerControlViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IWPFService _wpfService;
        private readonly System.Timers.Timer _timer;
        private SerialPort _serialPort;
        private double[] _yValues = new double[5];
        private double[] _zValues = new double[5];
        private double[] _xValues = new double[5];
        private int _valueIndex = 0;
        private Quaternion? quanterion;
        private int _valuesReceived = 0;
        private bool IsStandartData = false;

        public InclinometerControlViewModel(INavigationService navigationService, IWPFService wpfService)
        {
            _navigationService = navigationService;
            _wpfService = wpfService;
            // Инициализация доступных портов
            AvailablePorts = new ObservableCollection<string>();
            OxisY = "0";
            errorMessage = "";
            OxisX = "0";
            OxisZ = "0";
            // Установка начального статуса
            UpdateDeviceStatus(false);

            _timer = new System.Timers.Timer(1000); // 1000 мс = 1 секунда
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

        [ObservableProperty]
        private string temperature;

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
                        IsStandartData = true;
                        _valueIndex = 0;
                        _valuesReceived = 0;
                        _serialPort = new SerialPort(selectedPort, 115200);
                        _serialPort.DataReceived += OnDataReceived;
                        _serialPort.Open();
                        IsLoading = true;
                        _serialPort.WriteLine("");
                        _serialPort.WriteLine("get");
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

        [RelayCommand]
        private void ShowModel()
        {
            GetQuant();
            if (quanterion != null)
            {
                string[] parameters =
                [
                    quanterion.Value.X.ToString(),
                    quanterion.Value.Y.ToString(),
                    quanterion.Value.Z.ToString(),
                    quanterion.Value.W.ToString(),
                ];
                StartWpfApp(parameters);
            }
        }
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (IsStandartData)
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
                    if (double.TryParse(parts[1].Replace('.', ','), out double yValue))
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
                        GetTemp();
                        StopReading();
                        IsLoading = false;
                        IsStandartData = false;
                        return;
                    }
                    Thread.Sleep(1000);
                    _serialPort.WriteLine("get");
                }
            }
        }

        private void StopReading()
        {
            if(_serialPort != null)
            {
            _serialPort.Close();
            _serialPort = null;
            }
        }
        private void GetQuant()
        {
            if (selectedPort != null)
            {
                if (_serialPort == null)
                {
                    _serialPort = new SerialPort(selectedPort, 115200);
                    _serialPort.Open();
                }
                _serialPort.WriteLine("");
                _serialPort.WriteLine("quant");
                string data = _serialPort.ReadLine();
                var parts = data.Split(',');
                if (parts.Length >= 4)
                {
                    if (float.TryParse(parts[0].Replace('.', ','), out float w)
                        && float.TryParse(parts[1].Replace('.', ','), out float x)
                        && float.TryParse(parts[2].Replace('.', ','), out float y)
                        && float.TryParse(parts[3].Replace('.', ','), out float z))
                    {
                        if (w > 1.0 || w < -1.0
                            || x > 1.0 || x < -1.0
                            || y > 1.0 || y < -1.0
                            || z > 1.0 || z < -1.0)
                        {
                            GetQuant();
                        }
                        quanterion = new Quaternion(x, y, z, w);
                        StopReading();
                    }
                }
                else GetQuant();
            }
            else
            {
                ErrorMessage = "Выберите устройство и выполните подключение!";
                IsError = true;
            }
        }

        private void GetTemp()
        {
            if (selectedPort != null)
            {
                if (_serialPort == null)
                {
                    _serialPort = new SerialPort(selectedPort, 115200);
                    _serialPort.Open();
                }
                _serialPort.WriteLine("");
                _serialPort.WriteLine("temp");
                string data = _serialPort.ReadLine();
                var parts = data.Split(',');
                if (parts.Length >= 1)
                {
                    if (float.TryParse(parts[0].Replace('.', ','), out float temp))
                    {
                        Temperature = temp.ToString() + " °C";
                        StopReading();
                    }
                }
                else GetQuant();
            }
            else
            {
                ErrorMessage = "Выберите устройство и выполните подключение!";
                IsError = true;
            }
        }
        private void StartWpfApp(string[] parameters)
        {
            _wpfService.StartWpfApplication(parameters);
        }
        private void StopWpfApp()
        {
            _wpfService.StopWpfApplication();
        }
    }
}
