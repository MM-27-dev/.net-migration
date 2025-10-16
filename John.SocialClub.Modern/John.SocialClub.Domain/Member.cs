namespace John.SocialClub.Domain;

using System.ComponentModel.DataAnnotations;

public enum Occupation { Doctor = 1, Engineer, Professor }
public enum MaritalStatus { Married = 1, Single }
public enum HealthStatus { Excellent = 1, Good, Average, Poor }

public sealed class Member
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    [Required]
    public Occupation Occupation { get; set; }

    [Required]
    public MaritalStatus MaritalStatus { get; set; }

    [Required]
    public HealthStatus HealthStatus { get; set; }

    public decimal Salary { get; set; }
    public int NumberOfChildren { get; set; }
}
