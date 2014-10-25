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
using Caliburn.Micro;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.TED.CompositeLOBDemo
{
    public class CaliburnServiceLocator : IServiceLocator
    {
        private readonly DefaultWinRTContainer _container;

        public CaliburnServiceLocator(DefaultWinRTContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetSafeInstance(serviceType);
        }

        public object GetInstance(Type serviceType)
        {
            return _container.GetSafeInstance(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return _container.GetSafeInstance(serviceType, key);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        public T GetInstance<T>()
        {
            return (T)_container.GetSafeInstance(typeof(T));
        }

        public T GetInstance<T>(string key)
        {
            return (T)_container.GetSafeInstance(typeof(T), key);
        }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return _container.GetAllInstances<T>();
        }
    }
}