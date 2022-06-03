using System;
using System.Threading;
using System.Threading.Tasks;
using Services.Abstractions;
using System.Collections.Generic;
using Domain;
using Contracts;
using Contracts.Exceptions;
using System.Linq;

namespace Services
{
    internal sealed class MemberService : IMemberService
    {
        private readonly IRepositoryManager _repositoryManager;
        public MemberService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<MemberDTO> Authenticate(string username, string password)
        {
            var member = await _repositoryManager.MemberRepository.Authenticate(username, password);
            return new MemberDTO()
            {
                Id = member.Id.ToString(),
                Name = member.Name,
                Username = member.Username,
                Email = member.Email,
                Hours = member.Hours,
                Status = member.Status,
                Role = member.Role
            };
        }
        public async Task<int> SearchCountAsync(string search)
        {
                return await _repositoryManager.MemberRepository.SearchCountAsync(search);
        }
        public async Task<int> FilterCountAsync(string letter)
        {
            return await  _repositoryManager.MemberRepository.FilterCountAsync(letter);
        }
        public async Task<IEnumerable<MemberDTO>> FilterAsync(MemberParams memberParams, string letter)
        {
            var result = (await _repositoryManager.MemberRepository.FilterAsync(memberParams, letter)).Select(member => new MemberDTO()
            {
                Id = member.Id.ToString(),
                Name = member.Name,
                Username = member.Username,
                Email = member.Email,
                Hours = member.Hours,
                Status = member.Status,
                Role = member.Role
            });
            return result;
        }
        public async Task<IEnumerable<MemberDTO>> SearchAsync(MemberParams memberParams,string search)
        {
            var result = (await _repositoryManager.MemberRepository.SearchAsync(memberParams,search)).Select(member => new MemberDTO()
            {
                Id = member.Id.ToString(),
                Name = member.Name,
                Username = member.Username,
                Email = member.Email,
                Hours = member.Hours,
                Status = member.Status,
                Role = member.Role
            });
            return result;
        }
        public async Task CreateAsync(MemberDTO memberDTO, CancellationToken cancellationToken = default)
        {
              try
            {
            var member = new Member(Guid.NewGuid(),memberDTO.Name,memberDTO.Username,memberDTO.Email,memberDTO.Hours,memberDTO.Status,memberDTO.Role,memberDTO.Password); 
            await _repositoryManager.MemberRepository.InsertMember(member, cancellationToken);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
            }
            catch(EntityAlreadyExists)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try{
            var result = await _repositoryManager.MemberRepository.GetMemberById(id,cancellationToken);
            _repositoryManager.MemberRepository.RemoveMember(result);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
            }
            catch(NotFoundException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MemberDTO>> GetAllAsync(MemberParams memberParams,CancellationToken cancellationToken = default)
        {
            var result = (await _repositoryManager.MemberRepository.GetMembersAsync(memberParams,cancellationToken)).Select(member => new MemberDTO() {
            Id = member.Id.ToString(),
            Name = member.Name,
            Username = member.Username,
            Email = member.Email,
            Hours = member.Hours,
            Status = member.Status,
            Role = member.Role
        });
            return result;
        }

        public async Task<MemberDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
            var result = await _repositoryManager.MemberRepository.GetMemberById(id,cancellationToken);
            var memberDTO = new MemberDTO()
            {
                Id = result.Id.ToString(),
                Name = result.Name,
                Username = result.Username,
                Email = result.Email,
                Hours = result.Hours,
                Status = result.Status,
                Role = result.Role
            };
            return memberDTO;
            }
            catch(NotFoundException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task UpdateAsync(Guid id,MemberDTO memberDTO,CancellationToken cancellationToken = default)
        {
            try
            {
            //var checkhours = memberDTO.Hours > 0 ? memberDTO.Hours : 0;
            var memberforupdate = (await _repositoryManager.MemberRepository.GetMemberById(id,cancellationToken))
            .UpdateName(memberDTO.Name)
            .UpdateEmail(memberDTO.Email)
            .UpdateHours(memberDTO.Hours)
            .UpdateUserName(memberDTO.Username)
            .UpdateStatus(memberDTO.Status)
            .UpdateRole(memberDTO.Role);
            await _repositoryManager.MemberRepository.UpdateMember(memberforupdate,cancellationToken);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
            }
            catch(NotFoundException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}