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
using System.Reflection;
using System.Text;
using Caliburn.Micro;

namespace Microsoft.TED.CompositeLOBDemo
{
    public class DefaultWinRTContainer : WinRTContainer
    {
        public T GetSafeInstance<T>(Type type, string key = "") where T : class
        {
            return GetSafeInstance(type, key) as T;
        }

        public object GetSafeInstance(Type type, string key = "")
        {
            var obj = GetInstance(type, key);
            if (obj == null && type.GetTypeInfo().IsClass)
            {
                return this.BuildInstance(type);
            }
            return obj;
        }
    }
}
