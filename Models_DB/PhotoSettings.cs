using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VEGA.Models
{
     
    public class PhotoSettings
    {
        public long MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }

        public bool IsFileExtSupported(string fileExt) 
        {
            return (AcceptedFileTypes.Any(ft => ft.ToLower() == fileExt.ToLower())) ;
        }
 
    }
}
