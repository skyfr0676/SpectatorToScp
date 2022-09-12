using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScpDeconnexion
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        public string ScpDeconnextionTitle { get; set; } = "Scp Disconnect Alert";
        public string ScpDeconnextionDescription { get; set; } = " has disconnect and Replace by ";
        public int DescriptionSize { get; set; } = 30;
    }
}
