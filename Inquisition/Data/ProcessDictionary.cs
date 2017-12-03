using System.Collections.Generic;
using System.Diagnostics;

namespace Inquisition.Data
{
    public class ProcessDictionary
    {
        public static Dictionary<string, Process> Instance { get; set; } = new Dictionary<string, Process>();
    }
}
