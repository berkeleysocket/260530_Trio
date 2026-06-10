namespace Runtime.Networks
{
    public struct Account
    {
        public string Nickname { get; private set; }

        public Account(string nickname)
        {
            this.Nickname = nickname;
        }
    }
}
