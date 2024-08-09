using Avalonia.Controls;
using BoreholeSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Services
{
    public static class ViewLocator
    {
        public static UserControl GetView(ViewModelBase viewModel)
        {
            var viewTypeName = viewModel.GetType().FullName.Replace("ViewModel", "View");
            var viewType = Type.GetType(viewTypeName);
            if (viewType == null) return null;

            var view = Activator.CreateInstance(viewType) as UserControl;
            if (view != null)
            {
                view.DataContext = viewModel;
            }
            return view;
        }
    }
}
