namespace CleanArchitecture_2025.Domain.Abstraction
{
    public abstract class EntityDto
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public string CreateUserName { get; set; } = default!;
        public DateTimeOffset? UpdateAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public string? UpdateUserName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeleteAt { get; set; }
        public Guid? DeleteUserId { get; set; }
        public string? DeleteUserName { get; set; }
    }
}
