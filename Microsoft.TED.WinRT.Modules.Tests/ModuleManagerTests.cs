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
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.TED.WinRT.Modules.Tests.MockModules;
using Microsoft.TED.WinRT.Modules;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.TED.WinRT.Modules.Tests
{
    [TestClass]
    public class ModuleManagerTests
    {
        [TestMethod]
        public void InitializeAsync_ModuleMissing_Exception()
        {
            // ARRANGE
            ModuleManager manager = new ModuleManager();
            manager.AddModule(name: "Module1", entryPoint: "", dependsOn: new[] { "Module2", "Module3" });
            manager.AddModule(name: "Module2", entryPoint: "");

            // ACT
            try
            {
                manager.InitializeAsync().Wait();

                Assert.Fail("ModuleLoadException should have been thrown!");
            }
            catch (AggregateException ex)
            {
                if (!ex.InnerExceptions.OfType<ModuleLoadException>().Any()
                    && ex.InnerExceptions.Count == 1)
                {
                    Assert.Fail("ModuleLoadException should have been thrown!");
                }
            }

            // ASSERT

        }

        [TestMethod]
        public void InitializeAsync_ModulesLoaded_Success()
        {
            // ARRANGE
            ModuleManager manager = new ModuleManager();
            manager.AddModule(entryPoint: "Microsoft.TED.WinRT.Modules.Tests.MockModules.Module1,Microsoft.TED.WinRT.Modules.Tests", name: "Module1", dependsOn: new[] { "Module2" });
            manager.AddModule(name: "Module2", entryPoint: "Microsoft.TED.WinRT.Modules.Tests.MockModules.Module2,Microsoft.TED.WinRT.Modules.Tests", dependsOn: "Module3");
            manager.AddModule(name: "Module3", entryPoint: "Microsoft.TED.WinRT.Modules.Tests.MockModules.Module3,Microsoft.TED.WinRT.Modules.Tests");
            manager.AddModule(name: "Module4", entryPoint: "Microsoft.TED.WinRT.Modules.Tests.MockModules.Module4,Microsoft.TED.WinRT.Modules.Tests");
            manager.AddModule("Module5",
                "Microsoft.TED.WinRT.Modules.Tests.MockModules.Module5,Microsoft.TED.WinRT.Modules.Tests",
                "Module2", "Module4");

            List<string> initializedOrder = new List<string>();

            MockModuleBase.OnInitializeCalled = type => initializedOrder.Add(type.Name);

            // ACT
            manager.InitializeAsync().Wait();

            // ASSERT
            Assert.AreEqual(5, initializedOrder.Count, "Not all modules were initialized");
            Assert.AreEqual("Module3,Module4,Module2,Module5,Module1", initializedOrder.ConcatToString(","), "Invalid initialization order for modules");
        }


        [TestMethod]
        public void InitializeAsync_CircularDependency_Exception()
        {
            // ARRANGE
            ModuleManager manager = new ModuleManager();
            manager.AddModule(name: "Module1", entryPoint: "", dependsOn: new[] { "Module2", "Module3" });
            manager.AddModule(name: "Module2", entryPoint: "", dependsOn: "Module1");
            manager.AddModule(name: "Module3", entryPoint: "", dependsOn: "Module1");

            // ACT
            try
            {
                manager.InitializeAsync().Wait();

                Assert.Fail("CircularModuleDependencyException should have been thrown!");
            }
            catch (AggregateException ex)
            {
                if (!ex.InnerExceptions.OfType<CircularModuleDependencyException>().Any()
                                    || ex.InnerExceptions.Count != 1)
                {
                    Assert.Fail("CircularModuleDependencyException should have been thrown!");
                }

                Debug.WriteLine("Exception message: {0}", ex.InnerExceptions[0].Message);
            }
        }

        [TestMethod]
        public void InitializeAsync_FakeEntryPoints_Exception()
        {
            // ARRANGE
            ModuleManager manager = new ModuleManager();
            manager.AddModule(name: "Module1", entryPoint: "FakeModule1", dependsOn: "Module2");
            manager.AddModule(name: "Module2", entryPoint: "FakeModule2");

            // ACT
            try
            {
                manager.InitializeAsync().Wait();

                Assert.Fail("ModuleInitializeException should have been thrown!");
            }
            catch (AggregateException ex)
            {
                if (!ex.InnerExceptions.OfType<ModuleInitializeException>().Any()
                                    && ex.InnerExceptions.Count == 1)
                {
                    Assert.Fail("ModuleInitializeException should have been thrown!");
                }
            }
        }
    }
}
