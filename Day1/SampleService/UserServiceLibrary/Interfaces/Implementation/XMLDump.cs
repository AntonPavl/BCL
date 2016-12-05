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
    public class XMLDump : IDumper<User>
    {
        private string _filePath;
        public void Dump(IEnumerable<User> list,string path = null)
        {
            this._filePath = path;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (this._filePath == null)
                {
                    this._filePath = appSettings["FileName"] ?? null;
                    if (this._filePath == null) throw new ArgumentNullException();
                }
                var serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = new FileStream(_filePath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(fs, list);               
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("Error reading app settings");
            }
        }
        public IEnumerable<User> GetDump()
        {
            var formatter = new XmlSerializer(typeof(List<User>));
            IEnumerable<User> users;

            using (FileStream fs = new FileStream(this._filePath, FileMode.OpenOrCreate))
            {
                users = (IEnumerable<User>)formatter.Deserialize(fs);
            }

            return users;
        }
    }
}
