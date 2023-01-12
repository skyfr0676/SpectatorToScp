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
        public string ScpDeconnextionDescription { get; set; } = "{oldName} ({oldRole}) has disconnect and Replace by {name}";
        public int DescriptionSize { get; set; } = 30;
        public bool AntiPlayerDisconnect = true;
        public string ScpCantBeReplace = "{scp}({name]) has been disconnected but he cant be replace.";
    }
}
