namespace SocialNetwork.API.Entities.Chat;

public class OneToOneChat : Chat
{
    public Guid User1Id { get; set; }

    public Guid User2Id { get; set; }
}
