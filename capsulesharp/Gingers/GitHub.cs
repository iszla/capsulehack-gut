namespace capsulesharp.Gingers
{
    public class GitHub
    {
        User mUser;
        Rest mRest;

        public GitHub(string pRepoName, bool pPrivate)
        {
            mUser = User.GetUser("github");
            mRest = new Rest("https://api.github.com/", mUser.Username, mUser.Password);
            if (mRest.Post("user/repos", new JSONRequest(pRepoName, pPrivate)) == null)
                return;
            mRest.GitHubInit();
        }
    }

    class JSONRequest : RequestObject
    {
        public bool Private;
        public JSONRequest(string pName, bool pPrivate)
        {
            name = pName;
            Private = pPrivate;
        }
    }
}
