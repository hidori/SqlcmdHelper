#region LICENSE
// Copyright (c) 2016, Hiroaki SHIBUKI
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// * Neither the name of the {organization} nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Mjollnir.Testing.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Helps SQLCMD.exe automation.
    /// </summary>
    public class SqlcmdHelper
    {
        public SqlcmdHelper(string path, string server, string userId, string password, string database = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (password == null) throw new ArgumentNullException(nameof(password));
            // database can be null.

            this.ExecutablePath = path;
            this.Server = server;
            this.UserId = userId;
            this.Password = password;
            this.Database = database;
        }

        public SqlcmdHelper(string path, string server, string database = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (server == null) throw new ArgumentNullException(nameof(server));
            // database can be null.

            this.ExecutablePath = path;
            this.Server = server;
            this.UserId = null;
            this.Password = null;
            this.Database = database;
        }

        public string ExecutablePath { get; private set; }

        public string Server { get; private set; }

        public string UserId { get; private set; }

        public string Password { get; private set; }

        public string Database { get; private set; }

        public int ExecuteQueryString(string query, string database = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (database == null) throw new ArgumentNullException(nameof(database));

            return this.ExecuteQuery(() => this.CreateArguments(database).Concat(new[] { $"-Q \"{Escape(query)}\"" }));
        }

        public int ExecuteQueryString(string query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return this.ExecuteQuery(() => this.CreateArguments(this.Database).Concat(new[] { $"-Q \"{Escape(query)}\"" }));
        }

        public int ExecuteQueryFile(string fileName, string database)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (database == null) throw new ArgumentNullException(nameof(database));

            return this.ExecuteQuery(() => this.CreateArguments(database).Concat(new[] { $"-i \"{fileName}\"" }));
        }

        public int ExecuteQueryFile(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            return this.ExecuteQuery(() => this.CreateArguments(this.Database).Concat(new[] { $"-i \"{fileName}\"" }));
        }

        protected virtual int ExecuteQuery(Func<IEnumerable<string>> factory)
        {
            var arguments = string.Join(" ", factory());
            var process = Process.Start(this.ExecutablePath, arguments);

            process.WaitForExit();

            return process.ExitCode;
        }

        protected virtual IEnumerable<string> CreateArguments(string database)
        {
            // database can be null.

            yield return $"-S {this.Server}";

            yield return (this.UserId == null && this.Password == null)
                ? "-E"
                : $"-U {this.UserId} -P {this.Password}";

            if (database != null)
            {
                yield return $"-d {database}";
            }
        }

        static string Escape(string source)
        {
            return source.Replace(@"""", @"""""");
        }
    }
}
