﻿//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.
using System.Collections.Generic;
using System.Text;

namespace Microsoft.TED.WinRT.Modules
{
    public static class Helpers
    {
        public static string ConcatToString(this IEnumerable<object> source, string seperator)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var result in source)
            {
                sb.Append(result);
                sb.Append(seperator);
            }

            if (sb.Length > seperator.Length)
                sb.Remove(sb.Length - seperator.Length, seperator.Length);

            return sb.ToString();
        }
    }
}
