using BoreholeSystem.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Database
{
    public class DatabaseController
    {
        private ApplicationContext _applicationContext;
        public DatabaseController() 
        {
            _applicationContext = new ApplicationContext();
            _applicationContext.Database.EnsureCreated();
        }
        public void AddInclinometerData(DateTime dateTime, float x, float y, float z, float temp)
        {
            InclinometerModel model = new InclinometerModel()
            {
                X = x,
                DateTime = dateTime,
                Y = y,
                Z = z,
                Temp = temp
            };
            _applicationContext.InclinometersData.Add(model);
            _applicationContext.SaveChanges();
        }
        public List<InclinometerModel> GetInclinometersData()
        {
            return _applicationContext.InclinometersData.ToList();
        }
    }
}
