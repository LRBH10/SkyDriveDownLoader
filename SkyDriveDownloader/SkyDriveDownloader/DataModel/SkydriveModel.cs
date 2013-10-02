using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SkyDriveDownloader.DataModel
{

    public class Files
    {
        public List<FileDetails> data { get; set; }

        
    }
    
    public class FileDetails
    {

        public string id { get; set; }
        public string name { get; set; }
        public string parent_id { get; set; }
        public string description { get; set; }
        public long size { get; set; }
        public string type { get; set; }
        public string source { get; set; }
    }
}
