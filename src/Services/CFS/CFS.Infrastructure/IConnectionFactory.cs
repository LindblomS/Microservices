using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public interface IConnectionFactory
    {
        SqlConnection GetConnection();
    }
}
