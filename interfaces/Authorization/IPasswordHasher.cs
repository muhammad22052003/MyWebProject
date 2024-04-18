namespace WebProject.interfaces.auth
{
    public interface ICustomPasswordHasher
    {
        public string Generate(string password);

        public bool Verify(string password, string hashedPassword);
    }
}
