﻿//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.
using Windows.UI.Xaml.Controls;
using Microsoft.TED.CompositeLOBDemo.Module1.Views;
using Microsoft.TED.WinRT.ModularHub;
using Microsoft.TED.WinRT.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.Module1
{
    public class Module : IModule
    {
        private readonly IHubMetadataService _hubMetadataService;

        public Module(IHubMetadataService hubMetadataService)
        {
            _hubMetadataService = hubMetadataService;
        }

        public async Task InitializeAsync()
        {
            _hubMetadataService.AddHubSection("M1","Sheep", typeof(SheepListView).AssemblyQualifiedName);

            await Task.FromResult<object>(null);
        }
    }
}
