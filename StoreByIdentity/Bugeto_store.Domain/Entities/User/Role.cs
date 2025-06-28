public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserInRole> userInRoles { get; set; }
    }
