using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Utils
{
    public class WPFService : IWPFService
    {
        private string _wpfAppPath;
        private string _wpfAppName;

        public WPFService()
        {
            _wpfAppName = "BoreholeSystemModelVisualizator"; // Имя процесса WPF-приложения
            _wpfAppPath = @"C:\Users\lecha\source\repos\BoreholeSystem\BoreholeSystemModelVisualizator\bin\Debug\net8.0-windows\BoreholeSystemModelVisualizator.exe"; // Укажите путь к вашему WPF приложению
        }
        public void StartWpfApplication(string[] args)
        {
            // Проверяем, запущено ли WPF-приложение
            var runningProcess = GetRunningProcess();
            if (runningProcess != null)
            {
                // Завершаем работу приложения, если оно уже запущено
                runningProcess.Kill();
                runningProcess.WaitForExit();
            }

            // Формируем строку аргументов
            string arguments = string.Join(" ", args);

            // Настраиваем и запускаем процесс
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _wpfAppPath,
                Arguments = arguments
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
            }
        }

        public void StopWpfApplication()
        {
            var runningProcess = GetRunningProcess();
            if (runningProcess != null)
            {
                runningProcess.Kill();
                runningProcess.WaitForExit();
            }
        }

        public Process GetRunningProcess()
        {
            Process[] processes = Process.GetProcessesByName(_wpfAppName);
            return processes.Length > 0 ? processes[0] : null;
        }
    }
}
