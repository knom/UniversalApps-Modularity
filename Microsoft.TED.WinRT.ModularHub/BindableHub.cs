﻿//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.TED.WinRT.ModularHub
{
    public class BindableHub : Hub
    {
        public BindableHub()
        {
        }
        #region Dependency Properties

        #region DependencyProperty ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(BindableHub), new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((BindableHub)sender).OnItemsSourceChanged(e);
        }

        private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #region DependencyProperty ItemTemplate

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(BindableHub), new PropertyMetadata(default(DataTemplate), OnItemTemplateChanged));

        private static void OnItemTemplateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((BindableHub)sender).OnItemTemplateChanged(e);
        }

        private void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        #endregion

        #region DependencyProperty ItemTemplateSelector

        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register(
            "ItemTemplateSelector", typeof(DataTemplateSelector), typeof(BindableHub), new PropertyMetadata(default(DataTemplateSelector), OnItemTemplateSelectorChanged));

        private static void OnItemTemplateSelectorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((BindableHub)sender).OnItemTemplateSelectorChanged(e);
        }

        private void OnItemTemplateSelectorChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Refresh();
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        #endregion

        #region DependencyProperty HeaderPath

        public static readonly DependencyProperty HeaderPathProperty = DependencyProperty.Register(
            "HeaderPath", typeof(string), typeof(BindableHub), new PropertyMetadata(default(string), OnHeaderPathChanged));

        private static void OnHeaderPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((BindableHub)sender).OnHeaderPathChanged(e);
        }

        private void OnHeaderPathChanged(DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        public string HeaderPath
        {
            get { return (string)GetValue(HeaderPathProperty); }
            set { SetValue(HeaderPathProperty, value); }
        }

        #endregion

        #region DependencyProperty IsHeaderInteractivePath

        public static readonly DependencyProperty IsHeaderInteractivePathProperty = DependencyProperty.Register(
            "IsHeaderInteractivePath", typeof(string), typeof(BindableHub), new PropertyMetadata(default(string), OnIsHeaderInteractivePathChanged));

        private static void OnIsHeaderInteractivePathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((BindableHub)sender).OnIsHeaderInteractivePathChanged(e);
        }

        private void OnIsHeaderInteractivePathChanged(DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        public string IsHeaderInteractivePath
        {
            get { return (string) GetValue(IsHeaderInteractivePathProperty); }
            set { SetValue(IsHeaderInteractivePathProperty, value); }
        }

        #endregion

        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Refresh();
        }

        private void Refresh()
        {
            this.Sections.Clear();

            if (this.ItemsSource != null)
            {
                foreach (var item in this.ItemsSource)
                {
                    var section = GenerateHubSection(item);
                    
                    this.Sections.Add(section);
                }
            }
        }

        protected virtual HubSection GenerateHubSection(object item)
        {
            var section = new HubSection()
            {
                DataContext = item,
                Header = item,
                ContentTemplate = ItemTemplateSelector == null ? ItemTemplate : ItemTemplateSelector.SelectTemplate(item),
            };

            if (!String.IsNullOrEmpty(IsHeaderInteractivePath))
            {
                section.SetBinding(HubSection.IsHeaderInteractiveProperty, new Binding()
                {
                    Path = new PropertyPath(this.IsHeaderInteractivePath)
                });
            }
            if (!String.IsNullOrEmpty(HeaderPath))
            {
                section.SetBinding(HubSection.HeaderProperty, new Binding()
                {
                    Path = new PropertyPath(this.HeaderPath)
                });
            }

            return section;
        }
    }
}
