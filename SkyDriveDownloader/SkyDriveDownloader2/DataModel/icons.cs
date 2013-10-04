using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDriveDownloader2.Data
{
    public class Icons
    {
        public static Dictionary<string, string> map;
        

        public Icons()
        {
            map = new Dictionary<string, string>();

            map["default"] = "Assets/icons/default.png";

            map["folder"] = "Assets/icons/Folder.png";
            map["album"] = "Assets/icons/Photos.png";

            map[".pdf"] = "Assets/icons/PDF.png";
            
            map[".docx"] = "Assets/icons/docx.png";
            map[".pptx"] = "Assets/icons/pptx.png";
            map[".xlsx"] = "Assets/icons/xlsx.png";

            map[".txt"] = "Assets/icons/txt.png";
            
            map[".tex"] = "Assets/icons/tex.png";
            
            map[".zip"] = "Assets/icons/zip.png";
            map[".7z"] = "Assets/icons/zip.png";
            
        }

    }
}
