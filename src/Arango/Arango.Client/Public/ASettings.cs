using Arango.Client.Protocol;
using Arango.fastJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Arango.Client
{
    /// <summary>
    /// Represents ArangoDB client specific settings and pool of connections.
    /// </summary>
    public static class ASettings
    {
        private static readonly Dictionary<string, Connection> _Connections = new Dictionary<string, Connection>();

        internal static readonly Regex KeyRegex = new Regex(@"^[a-zA-Z0-9_\-:.@()+,=;$!*'%]*$");

        /// <summary>
        /// Determines driver name.
        /// </summary>
        public const string DriverName = "ArangoDB-NET";

        /// <summary>
        /// Determines driver version.
        /// </summary>
        public const string DriverVersion = "0.11.0";

        /// <summary>
        /// Determines JSON serialization options. Default value is { UseEscapedUnicode = false, UseFastGuid = false, UseExtensions = false }.
        /// </summary>
        public static JSONParameters JsonParameters { get; set; }

        /// <summary>
        /// Determines whether HTTP requests which return status code indicating an error (e.g. 4xx or 5xx) should also throw exceptions and not only contain error data within result object. Default value is false.
        /// </summary>
        public static bool ThrowExceptions { get; set; }

        static ASettings()
        {
            JsonParameters = new JSONParameters { UseEscapedUnicode = false, UseFastGuid = false, UseExtensions = false };
            ThrowExceptions = false;
        }

        #region AddConnection

        public static void AddConnection(string alias, string endpoint, bool useWebProxy = false)
        {
            AddConnection(alias, endpoint, "", "", useWebProxy);
        }

        public static void AddConnection(string alias, string endpoint, string username, string password, bool useWebProxy = false)
        {
            var connection = new Connection(alias, endpoint, username, password, useWebProxy);

            _Connections.Add(alias, connection);
        }

        public static void AddConnection(string alias, string endpoint, string databaseName, bool useWebProxy = false)
        {
            AddConnection(alias, endpoint, databaseName, "", "", useWebProxy);
        }

        public static void AddConnection(string alias, string endpoint, string databaseName, string username, string password, bool useWebProxy = false)
        {
            var connection = new Connection(alias, endpoint, databaseName, username, password, useWebProxy);

            _Connections.Add(alias, connection);
        }

        #endregion

        public static void RemoveConnection(string alias)
        {
            if (_Connections.ContainsKey(alias))
            {
                _Connections.Remove(alias);
            }
        }

        public static bool HasConnection(string alias)
        {
            return _Connections.ContainsKey(alias);
        }

        /// <summary>
        /// Get the name of the database specified in an specific Connection
        /// </summary>
        public static string GetConnectionDbName(string alias)
        {
            if (HasConnection(alias))
            {
                var conn = GetConnection(alias);
                return conn.DatabaseName;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"No Connection found with the name {alias}");
            }
        }

        public static string GetDefaultConnectionDbName()
        {
            if (_Connections.Count == 0)
                throw new ArgumentOutOfRangeException($"No Connections found");

            var conn = _Connections.FirstOrDefault(); 
            return conn.Value.DatabaseName;
        }

        internal static Connection GetConnection(string alias)
        {
            if (_Connections.ContainsKey(alias))
            {
                return _Connections[alias];
            }

            Debugger.Break();
            return null;
        }
    }
}