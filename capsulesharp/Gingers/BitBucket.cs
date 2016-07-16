namespace capsulesharp.Gingers
{
    public class BitBucket
    {
        User mUser;
        Rest mRest;

        public BitBucket(string pRepoName, bool pPrivate)
        {
            mUser = User.GetUser("bitbucket");
            mRest = new Rest("https://api.bitbucket.org/2.0/", mUser.Username, mUser.Password);
            if (mRest.Post("repositories/" + mUser.Username + "/" + pRepoName, new BBRequest(pRepoName, pPrivate)) == null)
                return;
            mRest.BitBucketInit();
        }
    }

    class BBRequest : RequestObject
    {
        public bool is_private;
        public string scm;
        public string fork_policy;

        public BBRequest(string pName, bool pPrivate)
        {
            name = pName;
            is_private = pPrivate;
            scm = "git";

            if (is_private)
            {
                fork_policy = "no_forks";
            }
            else
            {
                fork_policy = "allow_forks";
            }
        }
    }
}
