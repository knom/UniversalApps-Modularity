//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.TED.WinRT.ModularHub
{
    public class ViewModelHubSection : HubSection
    {
        const string XamlTemplate = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:views=\"using:{0}\"><views:{1}/></DataTemplate>";

        public ViewModelHubSection(Type contentControlType, string header = "", bool isHeaderInteractive = false)
        {
            this.Header = header;
            this.IsHeaderInteractive = isHeaderInteractive;

            string sectionXaml = String.Format(XamlTemplate,
                contentControlType.Namespace, contentControlType.Name);

            object style = null;
            Application.Current.Resources.TryGetValue(typeof(HubSection), out style);

            var hubSectionStyle = style as Style;
            if (hubSectionStyle != null)
                this.Style = hubSectionStyle;

            this.ContentTemplate = (DataTemplate)XamlReader.LoadWithInitialTemplateValidation(sectionXaml);

            this.Loaded += HubSection_Loaded;
        }

        void HubSection_Loaded(object sender, RoutedEventArgs e)
        {
            // Once the hubsection is loaded you can notify e.g. the ViewModel
            this.Loaded -= HubSection_Loaded;


            if (ServiceLocator.IsLocationProviderSet)
            {
                var serviceLocator = ServiceLocator.Current;
                //var view = serviceLocator.GetInstance(viewType) as Control;
                var view = FindFirstChild<UserControl>(this);
                if (view != null)
                {
                    var vmBinder = serviceLocator.GetInstance(typeof(IHubViewModelBinder)) as IHubViewModelBinder;
                    if (vmBinder != null)
                    {
                        var vm = vmBinder.BindAndActiveViewModel(view);
                    }
                }
            }
        }

        private static T FindFirstChild<T>(FrameworkElement element) where T:FrameworkElement
        {
            if (element == null) { return null; }

            if (element is T)
            {
                return (T)element;
            }

            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var result = FindFirstChild<T>((VisualTreeHelper.GetChild(element, i) as FrameworkElement));
                if (result != null) { return result; }
            }
            return null;
        }
    }
}