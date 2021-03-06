using System;
using System.Threading.Tasks;
using System.Threading;
using Domain.Services;

namespace Domain.Repositories
{
    public interface IRepositoryManager
    {
        ITimeSheetRepository TimeSheetRepository{get;}
        ICategoryRepository CategoryRepository{get;}
        IClientRepository ClientRepository {get;}
        IMemberRepository MemberRepository{get;}
        IProjectRepository ProjectRepository{get;}
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}