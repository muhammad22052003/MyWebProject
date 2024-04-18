namespace WebProject.Models
{
    public class User : IModel
    {
        private User(string id, string name, string hashedPassword, string email)
        {
            Id = id;
            Name = name;
            HashPassword = hashedPassword;
            Email = email;
            isBlocked = false;
        }

        public User()
        {
            
        }

        public string Id {  get; set; }

        public string Name { get; set; }

        public string HashPassword { get; set; }

        public string Email { get; set; }

        public string Role {  get; set; }

        public DateTime RegistrationTime {  get; set; }
        
        public DateTime LastLoginTime {  get; set; } 

        public bool isBlocked {  get; set; }

        public static User Create(string name, string hashedPassword, string email)
        {
            User user = new User(Guid.NewGuid().ToString().Replace("-",""), name, hashedPassword, email);

            return user;
        }
    }
}
