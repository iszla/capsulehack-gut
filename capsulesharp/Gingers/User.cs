using System;
using System.IO;
using System.Xml.Serialization;
using Encryption;

namespace capsulesharp.Gingers
{
    public class User
    {
        public string Username;
        public string Password;
        private static CryptService mCrypt = new CryptService(CryptProvider.DES);

        public User()
        {
        }

        public User(string pName, string pPass)
        {
            Username = pName;
            Password = pPass;
        }

        public static User GetUser(string pTarget)
        {
            var writer = new XmlSerializer(typeof(User));
            mCrypt.Key = "somethingRandom";

            var file = File.Open(AppDomain.CurrentDomain.BaseDirectory + "/" + pTarget + "user.xml", FileMode.Open);
            User lUser = (User)writer.Deserialize(file);
            lUser.Password = mCrypt.Decrypt(lUser.Password);

            file.Close();

            return lUser;
        }

        public static void CreateUser(string pTarget, string pUser, string pPass)
        {
            mCrypt.Key = "somethingRandom";
            pTarget = (pTarget == "1") ? "github" : "bitbucket";
            var writer = new XmlSerializer(typeof(User));
            var lFile = File.Open(AppDomain.CurrentDomain.BaseDirectory + "/" + pTarget + "user.xml", FileMode.Create);
            writer.Serialize(lFile, new User(pUser, mCrypt.Encrypt(pPass)));
            lFile.Close();
        }
    }
}
