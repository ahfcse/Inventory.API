namespace Inventory.API.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public string Email { get; }

        public DuplicateEmailException(string email)
            : base($"Email address '{email}' is already in use")
        {
            Email = email;
        }
    }
}
