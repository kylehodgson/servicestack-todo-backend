using System;
using ServiceStack;

namespace ToDoBackend
{
    public class HostConfigFactory
    {
        public const string DefaultBinding = "http://localhost:8888";

        public HostConfig GetHostConfig()
        {
            string primaryBinding = BindingFromEnvironment();
            return primaryBinding.Equals(string.Empty)
                ? new HostConfig { WebHostUrl = DefaultBinding }
                : new HostConfig { WebHostUrl = primaryBinding };
        }

        private string BindingFromEnvironment()
        {
            var host = Environment.GetEnvironmentVariable("todobackend_host");
            var port = Environment.GetEnvironmentVariable("todobackend_port");
            var protocol = Environment.GetEnvironmentVariable("todobackend_protocol");

            var binding = host.IsNullOrEmpty() || protocol.IsNullOrEmpty()
                       ? string.Empty
                       : AssembleBinding(protocol, host, port);

            return binding;
        }

        private static string AssembleBinding(string protocol, string host, string port)
        {
            if (port.IsNullOrEmpty())
            {
                return protocol + "://" + host;
            }
            else
            {
                return protocol + "://" + host + ":" + port;
            }
        }
    }
}