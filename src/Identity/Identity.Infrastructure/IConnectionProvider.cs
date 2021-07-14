namespace Services.Identity.Infrastructure
{
    using System.Data.SqlClient;

    public interface IConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
