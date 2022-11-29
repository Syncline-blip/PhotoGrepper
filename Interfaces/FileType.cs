using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGrepper.Interfaces
{
    interface IFileType
    {
        public string name { get; set; }
        public string fileSource { get; set; }
        public string fileNumber { get; set; }
    }
}
