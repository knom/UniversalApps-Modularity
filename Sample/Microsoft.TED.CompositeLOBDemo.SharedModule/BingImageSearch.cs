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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.SharedModule
{
    public class BingImageSearch : ISearchImages
    {
        public async Task<IEnumerable<Uri>> SearchAsync(string pattern)
        {
            string baseUrl = "http://www.bing.com/images/search?q={0}";

            HttpClient client = new HttpClient();

            string html = client.GetStringAsync(String.Format(baseUrl, pattern)).Result;

            var matches = Regex.Matches(html, "<img class=\"img_hid\" src2=\"([A-Za-z0-9\\.://?=]*)&amp;");

            var urls = matches.OfType<Match>().Select(p => new Uri(p.Groups[1].Value));

            return urls;
        }
    }
}
