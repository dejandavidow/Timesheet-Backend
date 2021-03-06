
using Contracts.Exceptions;
using Domain.Entities;
using Domain.Pagination;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class ProjectRepository : IProjectRepository
{
    private readonly RepositoryDbContext _dbContext;
    public ProjectRepository(RepositoryDbContext dbContext)
    {
        _dbContext=dbContext;
    }
    public async Task<int> CountSearchProjects(string search)
    {
        if(String.IsNullOrEmpty(search))
        {
            return await _dbContext.Projects.CountAsync();
        }
        return await _dbContext.Projects.Where(x => x.ProjectName.Contains(search)).CountAsync();
    }
    public async Task<int> CountFilterProjects(string letter)
    {
        return await _dbContext.Projects.Where(x => x.ProjectName.StartsWith(letter)).CountAsync();
    }
    public async Task<IEnumerable<Project>> SearchProjects(ProjectParams projectParams, string search)
    {
        return (await _dbContext.Projects
            .Where(x => x.ProjectName.Contains(search))
            .OrderBy(x => x.ProjectName)
            .Skip((projectParams.PageNumber - 1) * projectParams.PageSize)
            .Take(projectParams.PageSize)
            .Include(x => x.Client)
            .Include(x => x.Member).AsNoTracking()
            .ToListAsync())
            .Select(project => new Project(
            project.Id,
            project.ProjectName,
            project.Description,
            project.Archive,
            project.Status,
            new Client(project.Client.Id, project.Client.ClientName, project.Client.Adress, project.Client.City, project.Client.PostalCode, project.Client.Country),
            new Member(project.Member.Id, project.Member.Name, project.Member.Username, project.Member.Email, project.Member.Hours, project.Member.Status, project.Member.Role,project.Member.Password)
        ));
    }
    public async Task<IEnumerable<Project>> FilterProjects(ProjectParams projectParams,string letter)
    {
        return (await _dbContext.Projects
            .Where(x => x.ProjectName.StartsWith(letter))
            .OrderBy(x => x.ProjectName)
            .Skip((projectParams.PageNumber - 1) * projectParams.PageSize)
            .Take(projectParams.PageSize)
            .Include(x => x.Client)
            .Include(x => x.Member).AsNoTracking()
            .ToListAsync())
            .Select(project => new Project(
            project.Id,
            project.ProjectName,
            project.Description,
            project.Archive,
            project.Status,
            new Client(project.Client.Id, project.Client.ClientName, project.Client.Adress, project.Client.City, project.Client.PostalCode, project.Client.Country),
            new Member(project.Member.Id, project.Member.Name, project.Member.Username, project.Member.Email, project.Member.Hours, project.Member.Status, project.Member.Role,project.Member.Password)
        ));
    }
    public async Task<IEnumerable<Project>> GetProjectAsync(ProjectParams projectParams,CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Projects
            .OrderBy(x => x.ProjectName)
            .Skip((projectParams.PageNumber - 1) * projectParams.PageSize)
            .Take(projectParams.PageSize)
            .Include(x => x.Client)
            .Include(x => x.Member).AsNoTracking()
            .ToListAsync(cancellationToken))
            .Select(project => new Project(
            project.Id,
            project.ProjectName,
            project.Description,
            project.Archive,
            project.Status,
            new Client(project.Client.Id, project.Client.ClientName, project.Client.Adress, project.Client.City, project.Client.PostalCode, project.Client.Country),
            new Member(project.Member.Id, project.Member.Name, project.Member.Username, project.Member.Email, project.Member.Hours, project.Member.Status, project.Member.Role,project.Member.Password)
        ));
    }

    public async Task<Project> GetProjectById(Guid id, CancellationToken cancellationToken = default)
    {
        var persistenceproject = await _dbContext.Projects.Include(p => p.Client).Include(p => p.Member).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        if(persistenceproject == null)
        {
            throw new NotFoundException("Project not found.");
        }
        return new Project(persistenceproject.Id,persistenceproject.ProjectName,persistenceproject.Description,
        persistenceproject.Archive,persistenceproject.Status,new Client(persistenceproject.Client.Id, persistenceproject.Client.ClientName, persistenceproject.Client.Adress, persistenceproject.Client.City, persistenceproject.Client.PostalCode, persistenceproject.Client.Country),
            new Member(persistenceproject.Member.Id, persistenceproject.Member.Name, persistenceproject.Member.Username, persistenceproject.Member.Email, persistenceproject.Member.Hours, persistenceproject.Member.Status, persistenceproject.Member.Role,persistenceproject.Member.Password)
        );
    }

    public async Task InsertProject(Project project,CancellationToken cancellationToken)
    {
        var checkproject = await _dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == project.Id,cancellationToken);
        if(checkproject != null)
        {
            throw new EntityAlreadyExists("Project already exists");
        }
        await _dbContext.Projects.AddAsync(new Persistence.Models.PersistenceProject()
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            Archive = project.Archive,
            Status = project.Status,
            ClientId = project.Client.Id,
            MemberId = project.Member.Id
        });
    }

    public void RemoveProject(Project project)
    {
        _dbContext.Projects.Remove(new Persistence.Models.PersistenceProject(){
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            Archive = project.Archive,
              Client = new Persistence.Models.PersistenceClient()
            {
                Id = project.Client.Id,
                ClientName = project.Client.ClientName,
                Adress = project.Client.Adress,
                City = project.Client.City,
                PostalCode = project.Client.PostalCode,
                Country = project.Client.Country
            },
            Member = new Persistence.Models.PersistenceMember() 
            {
                Id = project.Member.Id,
                Name = project.Member.Name,
                Username = project.Member.Username,
                Email = project.Member.Email,
                Hours = project.Member.Hours,
                Status = project.Member.Status,
                Role = project.Member.Role
            }
            
        });
    }

    public async Task UpdateProject(Project project,CancellationToken cancellationToken)
    {
      var persistenceProject = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == project.Id,cancellationToken);
      persistenceProject.Id = project.Id;
      persistenceProject.ProjectName = project.ProjectName;
      persistenceProject.Description = project.Description;
      persistenceProject.Archive = project.Archive;
      persistenceProject.Status = project.Status;
      persistenceProject.ClientId = project.Client.Id;
      persistenceProject.MemberId = project.Member.Id;   
    }
}