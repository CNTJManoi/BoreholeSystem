using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Utils
{
    public interface IWPFService
    {
        public void StartWpfApplication(string[] args);

        public void StopWpfApplication();

        public Process GetRunningProcess();
    }
}
