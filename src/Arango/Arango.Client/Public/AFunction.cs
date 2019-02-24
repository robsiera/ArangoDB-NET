﻿using System.Collections.Generic;
using Dictator;
using Arango.Client.Protocol;
using Arango.fastJSON;

namespace Arango.Client
{
    public class AFunction
    {
        readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        readonly Connection _connection;
        
        internal AFunction(Connection connection)
        {
            _connection = connection;
        }
        
        #region Parameters
        
        /// <summary>
        /// Determines whether function return value solely depends on the input value and return value is the same for repeated calls with same input. This parameter is currently not applicable and may be used in the future for optimisation purpose.
        /// </summary>
        public AFunction IsDeterministic(bool value)
        {
            _parameters.Bool(ParameterName.IsDeterministic, value);
            
            return this;
        }
        
        /// <summary>
        /// Determines optional namespace from which to return all registered AQL user functions.
        /// </summary>
        public AFunction Namespace(string value)
        {
            _parameters.String(ParameterName.Namespace, value);
            
            return this;
        }
        
        /// <summary>
        /// Determines whether the function name is treated as a namespace prefix, and all functions in the specified namespace will be deleted. If set to false, the function name provided in name must be fully qualified, including any namespaces. Default value: false.
        /// </summary>
        public AFunction Group(bool value)
        {
            _parameters.String(ParameterName.Group, value.ToString().ToLower());
            
            return this;
        }
        
        #endregion
        
        #region Register (POST)

        /// <summary>
        /// Creates new or replaces existing AQL user function with specified name and code.
        /// </summary>
        public AResult<bool> Register(string name, string code)
        {
            var request = new Request(HttpMethod.POST, ApiBaseUri.AqlFunction, "");
            var bodyDocument = new Dictionary<string, object>();
            
            // required
            bodyDocument.String(ParameterName.Name, name);
            // required
            bodyDocument.String(ParameterName.Code, code);
            // optional
            Request.TrySetBodyParameter(ParameterName.IsDeterministic, _parameters, bodyDocument);
            
            request.Body = JSON.ToJSON(bodyDocument, ASettings.JsonParameters);
            
            var response = _connection.Send(request);
            var result = new AResult<bool>(response);
            
            switch (response.StatusCode)
            {
                case 200:
                case 201:
                    result.Success = true;
                    result.Value = true;
                    break;
                case 400:
                default:
                    // Arango error
                    break;
            }
            
            _parameters.Clear();
            
            return result;
        }
        
        #endregion
        
        #region List (GET)

        /// <summary>
        /// Retrieves list of registered AQL user functions.
        /// </summary>
        public AResult<List<Dictionary<string, object>>> List()
        {
            var request = new Request(HttpMethod.GET, ApiBaseUri.AqlFunction, "");
            
            // optional
            request.TrySetQueryStringParameter(ParameterName.Namespace, _parameters);
            
            var response = _connection.Send(request);
            var result = new AResult<List<Dictionary<string, object>>>(response);
            
            switch (response.StatusCode)
            {
                case 200:
                    //var body = ((IEnumerable)response.ParseBody()).Cast<Dictionary<string, object>>().ToList();
                    var body = response.ParseBody<List<Dictionary<string, object>>>();
                    
                    result.Success = (body != null);
                    result.Value = body;
                    break;
                default:
                    // Arango error
                    break;
            }
            
            _parameters.Clear();
            
            return result;
        }
        
        #endregion
        
        #region Unregister (DELETE)

        /// <summary>
        /// Unregisters specified AQL user function.
        /// </summary>
        public AResult<bool> Unregister(string name)
        {
            var request = new Request(HttpMethod.DELETE, ApiBaseUri.AqlFunction, "/" + name);
            
            // optional
            request.TrySetQueryStringParameter(ParameterName.Group, _parameters);
            
            var response = _connection.Send(request);
            var result = new AResult<bool>(response);
            
            switch (response.StatusCode)
            {
                case 200:
                    result.Success = true;
                    result.Value = true;
                    break;
                case 400:
                case 404:
                default:
                    // Arango error
                    break;
            }
            
            _parameters.Clear();
            
            return result;
        }
        
        #endregion
    }
}
