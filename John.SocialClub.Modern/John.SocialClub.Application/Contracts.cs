using John.SocialClub.Domain;

namespace John.SocialClub.Application;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Member>> SearchAsync(Occupation? occupation, MaritalStatus? maritalStatus, bool useAnd, CancellationToken ct = default);
    Task<Member> AddAsync(Member member, CancellationToken ct = default);
    Task<bool> UpdateAsync(Member member, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public interface IMemberService
{
    Task<Member?> GetAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Member>> ListAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Member>> SearchAsync(Occupation? occupation, MaritalStatus? maritalStatus, bool useAnd, CancellationToken ct = default);
    Task<Member> CreateAsync(Member member, CancellationToken ct = default);
    Task<bool> UpdateAsync(Member member, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class MemberService(IMemberRepository repo) : IMemberService
{
    private readonly IMemberRepository _repo = repo;

    public Task<Member?> GetAsync(int id, CancellationToken ct = default) => _repo.GetByIdAsync(id, ct);
    public Task<IReadOnlyList<Member>> ListAsync(CancellationToken ct = default) => _repo.GetAllAsync(ct);
    public Task<IReadOnlyList<Member>> SearchAsync(Occupation? occupation, MaritalStatus? maritalStatus, bool useAnd, CancellationToken ct = default)
        => _repo.SearchAsync(occupation, maritalStatus, useAnd, ct);

    public Task<Member> CreateAsync(Member member, CancellationToken ct = default)
        => _repo.AddAsync(member, ct);

    public Task<bool> UpdateAsync(Member member, CancellationToken ct = default)
        => _repo.UpdateAsync(member, ct);

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);
}
