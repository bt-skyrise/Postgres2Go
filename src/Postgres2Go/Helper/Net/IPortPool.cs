namespace Postgres2Go.Helper.Net
{
    public interface IPortPool
    {
        int GetNextOpenPort();
    }
}