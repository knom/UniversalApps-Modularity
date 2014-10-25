﻿//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using Caliburn.Micro;
using Microsoft.Practices.ServiceLocation;
using Microsoft.TED.CompositeLOBDemo.Views;
using Microsoft.TED.WinRT.ModularHub;
using Microsoft.TED.WinRT.Modules;

namespace Microsoft.TED.CompositeLOBDemo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : CaliburnApplication
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        private DefaultWinRTContainer _container;

        public ModuleManager ModuleManager { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            //this.Suspending += this.OnSuspending;

#if DEBUG
            LogManager.GetLog = type => new DebugLog(type);
#endif
            InitializeModules();
        }

        private void InitializeModules()
        {
            this.ModuleManager = new ModuleManager();
            this.ModuleManager.AddModule(name: "Module1",
                entryPoint: "Microsoft.TED.CompositeLOBDemo.Module1.Module,Microsoft.TED.CompositeLOBDemo.Module1",
                dependsOn: "SharedModule");

            this.ModuleManager.AddModule(name: "Module2",
                entryPoint: "Microsoft.TED.CompositeLOBDemo.Module2.Module,Microsoft.TED.CompositeLOBDemo.Module2",
                dependsOn: "SharedModule");

            this.ModuleManager.AddModule(name: "SharedModule",
                entryPoint: "Microsoft.TED.CompositeLOBDemo.SharedModule.Module,Microsoft.TED.CompositeLOBDemo.SharedModule");
        }

        protected override void Configure()
        {
            _container = new DefaultWinRTContainer();
            _container.RegisterInstance(typeof(DefaultWinRTContainer), "", _container);
            _container.RegisterWinRTServices();
            ServiceLocator.SetLocatorProvider(() => new CaliburnServiceLocator(_container));

            // Hub metadata service
            _container.Singleton<IHubMetadataService, HubMetadataService>();
            _container.RegisterSingleton(typeof(IHubViewModelBinder), "", typeof(CaliburnHubViewModelBinder));

            // Modules
            _container.RegisterInstance(typeof(ModuleManager), "", this.ModuleManager);
            this.ModuleManager.InitializeAsync().Wait();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var result = base.SelectAssemblies().ToList();
            result.AddRange(this.ModuleManager.LoadModuleAssemblies());

            return result;
        }

        #region WinRT Container Init
        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetSafeInstance(service, key);
            if (instance != null)
                return instance;
            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
        #endregion

        #region Navigation Init
        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }
        #endregion

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<MainPage>();
        }

        ///// <summary>
        ///// Invoked when the application is launched normally by the end user.  Other entry points
        ///// will be used when the application is launched to open a specific file, to display
        ///// search results, and so forth.
        ///// </summary>
        ///// <param name="e">Details about the launch request and process.</param>
        //        protected override void OnLaunched(LaunchActivatedEventArgs e)
        //        {
        //#if DEBUG
        //            if (System.Diagnostics.Debugger.IsAttached)
        //            {
        //                this.DebugSettings.EnableFrameRateCounter = true;
        //            }
        //#endif

        //            Frame rootFrame = Window.Current.Content as Frame;

        //            // Do not repeat app initialization when the Window already has content,
        //            // just ensure that the window is active
        //            if (rootFrame == null)
        //            {
        //                // Create a Frame to act as the navigation context and navigate to the first page
        //                rootFrame = new Frame();

        //                // TODO: change this value to a cache size that is appropriate for your application
        //                rootFrame.CacheSize = 1;

        //                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        //                {
        //                    // TODO: Load state from previously suspended application
        //                }

        //                // Place the frame in the current Window
        //                Window.Current.Content = rootFrame;
        //            }

        //            if (rootFrame.Content == null)
        //            {
        //#if WINDOWS_PHONE_APP
        //                // Removes the turnstile navigation for startup.
        //                if (rootFrame.ContentTransitions != null)
        //                {
        //                    this.transitions = new TransitionCollection();
        //                    foreach (var c in rootFrame.ContentTransitions)
        //                    {
        //                        this.transitions.Add(c);
        //                    }
        //                }

        //                rootFrame.ContentTransitions = null;
        //                rootFrame.Navigated += this.RootFrame_FirstNavigated;
        //#endif

        //                // When the navigation stack isn't restored navigate to the first page,
        //                // configuring the new page by passing required information as a navigation
        //                // parameter
        //                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
        //                {
        //                    throw new Exception("Failed to create initial page");
        //                }
        //            }

        //            // Ensure the current window is active
        //            Window.Current.Activate();
        //        }

        //#if WINDOWS_PHONE_APP
        //        /// <summary>
        //        /// Restores the content transitions after the app has launched.
        //        /// </summary>
        //        /// <param name="sender">The object where the handler is attached.</param>
        //        /// <param name="e">Details about the navigation event.</param>
        //        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        //        {
        //            var rootFrame = sender as Frame;
        //            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
        //            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        //        }
        //#endif
    }
}