using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SkyDriveDownloader2.Data
{

    public class Files
    {
        public FileDetails Identifier { get; set; }
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
            string ret = "";
            foreach (FileDetails s in data)
            {
                ret += s.name;
            }
            return ret;
        }

        public async static Task<SampleDataGroup> GetDirectoryView(FileDetails current_dir)
        {
            if (LiveConfig.LoginResult != null)
            {
                LiveOperationResult operationResult = await LiveConfig.ConnectClient.GetAsync(current_dir.id + "/files");
                dynamic result = operationResult.RawResult;
                if (result != null)
                {
                    Files re = Files.GetResponseApiFrom(result);
                    re.Identifier = current_dir;
                    var resultGroup = re.GetGroupView();
                    SampleDataSource.GetInstance.AllGroups.Add(resultGroup);
                    return resultGroup;
                }
                else
                {
                    MessageDialog x = new MessageDialog("Error : No Information  getted from Server ");
                    await x.ShowAsync();
                    return null;
                }
            }
            else
            {
                MessageDialog x = new MessageDialog("Error : Login result ");
                await x.ShowAsync();
                return null;
            }
        }


        public SampleDataGroup GetGroupView()
        {
            SampleDataGroup res = new SampleDataGroup(Identifier.id, Identifier.name, Identifier.description, "", "");
            foreach (FileDetails f in data)
            {
                f.AddItemView(res);
            }
            return res;
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


        public void AddItemView(SampleDataGroup gr)
        {
            var item = new SampleDataItem(id,
                    name,
                    description,
                    "Assets/MediumGray.png",
                    source,
                    type,
                    gr);

            item.Data = this;
            gr.Items.Add(item);
        }


        
    }
}
