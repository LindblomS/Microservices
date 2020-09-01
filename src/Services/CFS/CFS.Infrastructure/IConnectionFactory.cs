using System.Data;
using System.Threading.Tasks;

namespace CFS.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
