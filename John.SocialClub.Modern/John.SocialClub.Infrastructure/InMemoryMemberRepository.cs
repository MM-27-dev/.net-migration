using John.SocialClub.Application;
using John.SocialClub.Domain;

namespace John.SocialClub.Infrastructure;

public sealed class InMemoryMemberRepository : IMemberRepository
{
    private static readonly List<Member> _members;
    private static int _nextId = 1;
    private static readonly object _gate = new();

    static InMemoryMemberRepository()
    {
        _members = Seed();
        if (_members.Count > 0) _nextId = _members.Max(m => m.Id) + 1;
    }

    public Task<Member?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_members.FirstOrDefault(m => m.Id == id));

    public Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult((IReadOnlyList<Member>)_members.OrderBy(m => m.Id).ToList());

    public Task<IReadOnlyList<Member>> SearchAsync(Occupation? occupation, MaritalStatus? maritalStatus, bool useAnd, CancellationToken ct = default)
    {
        IEnumerable<Member> q = _members;
        if (useAnd)
        {
            if (occupation.HasValue) q = q.Where(m => m.Occupation == occupation);
            if (maritalStatus.HasValue) q = q.Where(m => m.MaritalStatus == maritalStatus);
        }
        else
        {
            if (occupation.HasValue && maritalStatus.HasValue)
            {
                q = q.Where(m => m.Occupation == occupation || m.MaritalStatus == maritalStatus);
            }
            else if (occupation.HasValue)
            {
                q = q.Where(m => m.Occupation == occupation);
            }
            else if (maritalStatus.HasValue)
            {
                q = q.Where(m => m.MaritalStatus == maritalStatus);
            }
        }
        return Task.FromResult((IReadOnlyList<Member>)q.ToList());
    }

    public Task<Member> AddAsync(Member member, CancellationToken ct = default)
    {
        lock (_gate)
        {
            member.Id = _nextId++;
            _members.Add(member);
            return Task.FromResult(member);
        }
    }

    public Task<bool> UpdateAsync(Member member, CancellationToken ct = default)
    {
        lock (_gate)
        {
            var idx = _members.FindIndex(m => m.Id == member.Id);
            if (idx < 0) return Task.FromResult(false);
            _members[idx] = member;
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        lock (_gate)
        {
            var removed = _members.RemoveAll(m => m.Id == id) > 0;
            return Task.FromResult(removed);
        }
    }

    private static List<Member> Seed()
    {
        var rnd = new Random(12345);
        string[] firstNames = { "Alex", "Jamie", "Taylor", "Morgan", "Jordan", "Casey", "Riley", "Avery", "Dakota", "Reese" };
        string[] lastNames = { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin" };

        var list = new List<Member>();
        for (int i = 1; i <= 25; i++)
        {
            string name = $"{firstNames[rnd.Next(firstNames.Length)]} {lastNames[rnd.Next(lastNames.Length)]}";
            int age = rnd.Next(18, 71);
            var dob = DateTime.Today.AddYears(-age).AddDays(rnd.Next(0, 365));
            var occupation = (Occupation)rnd.Next(1, 3 + 1);
            var marital = (MaritalStatus)rnd.Next(1, 2 + 1);
            var health = (HealthStatus)rnd.Next(1, 4 + 1);
            decimal salary = Math.Round((decimal)(rnd.NextDouble() * (150000 - 30000) + 30000), 2);
            int children = rnd.Next(0, 7);

            list.Add(new Member
            {
                Id = i,
                Name = name,
                DateOfBirth = dob,
                Occupation = occupation,
                MaritalStatus = marital,
                HealthStatus = health,
                Salary = salary,
                NumberOfChildren = children
            });
        }
        return list;
    }
}
