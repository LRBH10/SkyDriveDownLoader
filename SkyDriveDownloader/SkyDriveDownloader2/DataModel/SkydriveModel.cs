using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SkyDriveDownloader2.Data
{

    public class Files
    {
        public List<FileDetails> data { get; set; }


        public static Files GetResponseApiFrom(string str)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(Files));
            MemoryStream stre = new MemoryStream(Encoding.UTF8.GetBytes(str));
            Files a = (Files)s.ReadObject(stre);
            return a;
        }

        public override string ToString()
        {
            string ret="";
            foreach (FileDetails s in data)
            {
                ret += s.name;
            }

            return ret;
        }
        
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

        public override string ToString()
        {
            return name;
        }

        public static FileDetails GetResponseApiFrom(string str)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(FileDetails));
            MemoryStream stre = new MemoryStream(Encoding.UTF8.GetBytes(str));
            FileDetails a = (FileDetails)s.ReadObject(stre);
            return a;
        }
    }
}
