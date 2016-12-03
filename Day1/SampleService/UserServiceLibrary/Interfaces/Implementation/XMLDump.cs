using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using UserServiceLibrary.Interfaces.Implementations;

namespace UserServiceLibrary.Interfaces.Implementation
{
    public class XMLDump : IDump
    {
        public void Dump(IUserStorageService uss)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var fileName = appSettings["FileName"] ?? "Not Found";
                var serializer = new XmlSerializer(typeof(UserStorageService));
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(fs, uss);               
                }
            }
            catch (ConfigurationErrorsException)
            {
                throw new ConfigurationErrorsException("Error reading app settings");
            }
        }
    }
}
