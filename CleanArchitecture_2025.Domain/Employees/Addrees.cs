namespace CleanArchitecture_2025.Domain.Employees;

public sealed record Addrees
{
 
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Town { get; set; }
    public string? FullAddress { get; set; }

}
