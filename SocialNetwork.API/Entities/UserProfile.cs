namespace SocialNetwork.API.Entities
{
    public class UserProfile
    {
        public Guid UserId { get; set; }

        public String Name { get; set; }

        public String Avatar { get; set; }

        public String Cover { get; set; }

        public String DateOfBirth { get; set; }

        public String CurrentLocation { get; set; }

        public String SelfIntroduction { get; set; }

        public bool Gender { get; set; }
    }
}
