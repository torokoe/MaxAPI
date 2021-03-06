/*
Copyright 2011 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;

namespace Google.Apis.Util
{
    /// <summary>
    /// An attribute which is used to specially mark a property for reflective purposes, 
    /// assign a name to the property and indicate it's location in the request as either
    /// in the path or query portion of the request URL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequestParameterAttribute : Attribute
    {
        private readonly string name;
        private readonly RequestParameterType type;

        /// <summary>Gets the name of the parameter.</summary>
        public string Name { get { return name; } }

        /// <summary>Gets the type of the parameter, Path or Query.</summary>
        public RequestParameterType Type { get { return type; } }

        /// <summary>
        /// Constructs a new property attribute to be a part of a REST URI. 
        /// This constructor uses <see cref="RequestParameterType.Query"/> as the parameter's type.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter. If the parameter is a path parameter this name will be used to substitute the 
        /// string value into the path, replacing {name}. If the parameter is a query parameter, this parameter will be
        /// added to the query string, in the format "name=value".
        /// </param>
        public RequestParameterAttribute(string name)
            : this(name, RequestParameterType.Query)
        {

        }

        /// <summary>Constructs a new property attribute to be a part of a REST URI.</summary>
        /// <param name="name">
        /// The name of the parameter. If the parameter is a path parameter this name will be used to substitute the 
        /// string value into the path, replacing {name}. If the parameter is a query parameter, this parameter will be
        /// added to the query string, in the format "name=value".
        /// </param>
        /// <param name="type">The type of the parameter, either Path, Query or UserDefinedQueries.</param>
        public RequestParameterAttribute(string name, RequestParameterType type)
        {
            this.name = name;
            this.type = type;
        }
    }

    /// <summary>Describe the type of this parameter (Path, Query or UserDefinedQueries).</summary>
    public enum RequestParameterType
    {
        /// <summary>A path parameter which is inserted into the path portion of the request URI.</summary>
        Path,

        /// <summary>A query parameter which is inserted into the query portion of the request URI.</summary>
        Query,

        /// <summary>
        /// A group of user-defined parameters that will be added in to the query portion of the request URI. If this
        /// type is being used, the name of the RequestParameterAttirbute is meaningless.
        /// </summary>
        UserDefinedQueries
    }
}
