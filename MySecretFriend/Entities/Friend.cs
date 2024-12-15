namespace MySecretFriend.Entities
{
    public class Friend
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Nome { get; set; } = string.Empty;
        public string Amigo { get; set; } = string.Empty;
        public string? Dica1 { get; set; }
        public string? Dica2 { get; set; }
        public string? Dica3 { get; set; }
        public string? Dica4 { get; set; }
    }
}
