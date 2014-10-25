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
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.TED.WinRT.Modules
{
    /// <summary>
    /// Builds a repository of modules and initializes them.
    /// </summary>
    /// <remarks>Build your own inherited ModuleManager to accomodate custom IOC containers and type resolvers.</remarks>
    public class ModuleManager
    {
        private readonly Dictionary<string, ModuleInfo> _modules = new Dictionary<string, ModuleInfo>();

        public void AddModule(string name, string entryPoint, params string[] dependsOn)
        {
            var info = new ModuleInfo(name, entryPoint, dependsOn);
            _modules.Add(name, info);
        }

        public async Task InitializeAsync()
        {
            var orderedResult = ResolveModuleOrder();

            foreach (var moduleInfo in orderedResult)
            {
                var type = ResolveModuleType(moduleInfo);
                if (type == null)
                    throw new ModuleInitializeException(string.Format("Module {0} ({1}) wasn't found", moduleInfo.Name, moduleInfo.EntryPoint));

                IModule module = CreateModuleInstance(type);
                if (module == null)
                    throw new ModuleInitializeException(string.Format("Module {0} ({1}) couldn't be constructed. Maybe it's not implementing IModule?", moduleInfo.Name, moduleInfo.EntryPoint));

                await module.InitializeAsync();
            }
        }

        private IEnumerable<ModuleInfo> ResolveModuleOrder()
        {
            // Check for modules missing
            var notInList = _modules.Values.SelectMany(p => p.DependsOn).Except(_modules.Keys).ToList();

            if (notInList.Any())
                throw new ModuleLoadException(
                    String.Format(
                        "Modules {0} were requested as a dependency by other modules but not registered with the ModuleManager!",
                        notInList.ConcatToString(", ")));

            // Fetch Modules w. no dependencies
            List<ModuleInfo> orderedResult = _modules.Where(p => !p.Value.DependsOn.Any()).Select(p => p.Value).ToList();

            if (orderedResult.Count == 0)
            {
                var circularModules =
                    _modules.Values.SelectMany(m1 => m1.DependsOn.Where(m2 => _modules[m2].DependsOn.Contains(m1.Name)))
                        .Distinct();
                throw new CircularModuleDependencyException(string.Format("Modules have circular dependencies: {0}",
                    circularModules.ConcatToString(", ")));
            }

            while (orderedResult.Count < _modules.Count)
            {
                foreach (var module in _modules)
                {
                    if (orderedResult.Contains(module.Value)) continue;

                    // Not in the list yet
                    // But all it's dependencies are in the list
                    if (module.Value.DependsOn.All(dependents => orderedResult.Any(p => p.Name == dependents)))
                        orderedResult.Add(module.Value);
                }
            }
            return orderedResult;
        }

        protected virtual IModule CreateModuleInstance(Type type)
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return (IModule)ServiceLocator.Current.GetInstance(type);
            }
            else
                return Activator.CreateInstance(type) as IModule;
        }

        protected virtual Type ResolveModuleType(ModuleInfo moduleInfo)
        {
            return Type.GetType(moduleInfo.EntryPoint);
        }

        public IEnumerable<Assembly> LoadModuleAssemblies()
        {
            var result = new List<Assembly>();

            foreach (var moduleInfo in _modules)
            {
                var type = Type.GetType(moduleInfo.Value.EntryPoint);
                if (type == null)
                    throw new ModuleLoadException(string.Format("Module {0} wasn't found!", moduleInfo.Value.Name));

                result.Add(type.GetTypeInfo().Assembly);
            }
            return result;
        }
    }
}
