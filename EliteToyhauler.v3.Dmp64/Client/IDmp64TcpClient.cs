using System.Threading.Tasks;

namespace EliteToyhauler.v3.Dmp64.Client
{
    public interface IDmp64TcpClient
    {
        Task<string> SendAsync(string message); 
    }
}