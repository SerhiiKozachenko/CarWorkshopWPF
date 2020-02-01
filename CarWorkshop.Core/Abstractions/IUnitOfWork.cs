using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
