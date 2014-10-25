﻿//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.TED.CompositeLOBDemo.SharedModule;

namespace Microsoft.TED.CompositeLOBDemo.Module2.ViewModels
{
    public class CatListViewModel : Screen
    {
        private readonly ISearchImages _imageSearchAgent;
        public ObservableCollection<Cat> Items { get; set; }

        public CatListViewModel(ISearchImages imageSearchAgent)
        {
            _imageSearchAgent = imageSearchAgent;
            this.Items = new ObservableCollection<Cat>();
        }

        protected override void OnActivate()
        {
            RefreshAsync();
        }

        private void RefreshAsync()
        {
            Task.Run(() =>
            {
                var images = _imageSearchAgent.SearchAsync("Cat").Result
                    .Take(10).ToList();
                var result = new ObservableCollection<Cat>();

                for (int i = 0; i < 9; i++)
                {
                    result.Add(new Cat()
                    {
                        Name = string.Format("Cat {0}", i + 1),
                        Picture = images[i]
                    });
                }

                Execute.OnUIThread(() => this.Items = result);
            });
        }
    }
}