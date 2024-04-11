using Application.DTOs.RepositoryDTOs;
using Domain.Entities;

namespace Application.Repositories;

public interface IFileRepository
{
    Task<byte[]> GetContent(long fileId);
    Task<MyFileInfoDto> GetInfo(long fileId);
    Task<long> GetOwner(long fileId);
    void AddForUser(MyFile file, long userId);
    void Remove(long fileId);
}