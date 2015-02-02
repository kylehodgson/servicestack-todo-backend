using System.Collections.Generic;

namespace ToDoBackend
{
    public interface IBindingManager
    {
        IEnumerable<string> GetBindings();
    }

    public class IISBindingManager : IBindingManager
    {
        public IEnumerable<string> GetBindings()
        {
            var siteName = System.Web.Hosting.HostingEnvironment.SiteName;
            var sitesSection =
                Microsoft.Web.Administration.WebConfigurationManager
                         .GetSection(null, null, "system.applicationHost/sites");

            foreach (var site in sitesSection.GetCollection())
            {
                if (site["name"].Equals(siteName))
                {
                    foreach (var binding in site.GetCollection("bindings"))
                    {
                        var protocol = (string)binding["protocol"];
                        var bindingInfo = (string)binding["bindingInformation"];

                        if (protocol.ToLower().StartsWith("http"))
                        {
                            string[] parts = bindingInfo.Split(':');
                            if (parts.Length == 3)
                            {
                                string port = parts[1];
                                string host = parts[2];
                                yield return protocol + "://" + host + ":" + port;
                            }
                        }
                    }
                }
            }
        }
    }
}