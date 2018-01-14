namespace Postgres2Go.Helper.Net
{
    public interface IPortWatcher
    {
        int FindOpenPort(int startPort);
    }
}