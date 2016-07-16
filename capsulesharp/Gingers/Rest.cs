using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System;

namespace capsulesharp.Gingers
{
    public class Rest
    {
        RestClient mClient;
        string mUser;
        string mPass;
        string mName;

        public Rest(string pBase, string user, string pass)
        {
            mClient = new RestClient(pBase);
            mClient.Authenticator = new HttpBasicAuthenticator(user, pass);
            mUser = user;
            mPass = pass;
        }

        public Dictionary<string, string> Post(string pResource, RequestObject pParams)
        {
            RestRequest lRequest = new RestRequest(pResource, Method.POST);
            mName = pParams.Name;

            var lJson = lRequest.JsonSerializer.Serialize(pParams);
            lJson = lJson.ToLower();
            lRequest.AddParameter("application/json; charset=utf-8", lJson, ParameterType.RequestBody);

            IRestResponse lResponse = mClient.Execute(lRequest);

            if (lResponse.StatusCode != System.Net.HttpStatusCode.OK && lResponse.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("An error occured. Exiting");
                Console.WriteLine(lResponse.StatusCode.ToString());
                Console.WriteLine(lResponse.ErrorMessage);
                return null;
            }

            RestSharp.Deserializers.JsonDeserializer lDeserializer = new RestSharp.Deserializers.JsonDeserializer();
            Dictionary<string, string> lResp = lDeserializer.Deserialize<Dictionary<string, string>>(lResponse);

            return lResp;
        }

        public void GitHubInit()
        {
            System.Diagnostics.Process.Start("cmd.exe",
                "/c git init&git add .&git commit -m \"Inital commit\"&git remote add origin https://" + mUser + "@github.com/" + mUser +
                "/" + mName + ".git&git push https://" + mUser + ":" + mPass + "@github.com/" + mUser + "/" + mName + ".git");
        }

        public void BitBucketInit()
        {
            System.Diagnostics.Process.Start("cmd.exe",
                "/c git init&git add .&git commit -m \"Inital commit\"&git remote add origin https://" + mUser + "@bitbucket.org/" + mUser +
                "/" + mName + ".git&git push https://" + mUser + ":" + mPass + "@bitbucket.org/" + mUser + "/" + mName + ".git");
        }
    }
}
