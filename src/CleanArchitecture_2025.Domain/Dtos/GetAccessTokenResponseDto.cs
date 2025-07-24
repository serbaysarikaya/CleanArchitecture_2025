using System.Text.Json.Serialization;

namespace CleanArchitecture_2025.Domain.Dtos;

public sealed class GetAccessTokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;
}


public sealed class CreateRoleDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;
}
public sealed class RoleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
}
public sealed record UpdateUserDto
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}


public sealed class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("createdTimestamp")]
    public long CreatedTimestamp { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}