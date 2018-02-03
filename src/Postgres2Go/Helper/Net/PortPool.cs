﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Postgres2Go.Helper.Net
{
    public sealed class PortPool : IPortPool
    {
        private readonly Object _lock = new Object();
        private static readonly PortPool Instance = new PortPool();

        private int _startPort = PostgresDefaults.TcpPort + 10000;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PortPool()
        {
        }

        // Singleton
        private PortPool()
        {
        }

        public static PortPool GetInstance
        {
            get
            {
                return Instance;
            }
        }

        /// <summary>
        /// Returns and reserves a new port
        /// </summary>
        public int GetNextOpenPort()
        {
            lock (_lock)
            {
                IPortWatcher portWatcher = PortWatcherFactory.CreatePortWatcher();
                int newAvailablePort = portWatcher.FindOpenPort(_startPort);
                
                return newAvailablePort;
            }
        }
    }
}
