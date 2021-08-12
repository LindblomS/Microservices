namespace Services.User.Infrastructure
{
    using System.Data.SqlClient;

    public interface IConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
