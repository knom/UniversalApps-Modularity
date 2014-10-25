using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Microsoft.TED.WinRT.ModularHub;

namespace Microsoft.TED.CompositeLOBDemo
{
    public class CaliburnHubViewModelBinder : IHubViewModelBinder
    {
        public object BindAndActiveViewModel(Control view)
        {
            var viewModel = ViewModelLocator.LocateForViewType(view.GetType());

            ViewModelBinder.Bind(viewModel, view, null);

            var activate = viewModel as IActivate;
            if (activate != null)
                activate.Activate();

            return viewModel;
        }
    }
}
