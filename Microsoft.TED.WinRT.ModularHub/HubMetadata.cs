﻿//	Copyright (c) Max Knor, Microsoft
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

namespace Microsoft.TED.WinRT.ModularHub
{
    public class HubMetadata
    {
        public HubMetadata(string sectionHeader, string sectionControl, string headerNavigateToPage, int priority)
        {
            SectionHeader = sectionHeader;
            SectionControl = sectionControl;
            HeaderNavigateToPage = headerNavigateToPage;
            Priority = priority;
        }

        public string SectionHeader { get; private set; }
        public string SectionControl { get; private set; }
        public string HeaderNavigateToPage { get; private set; }
        public int Priority { get; private set; }
    }
}
