using MySecretFriend.Entities;

namespace MySecretFriend.Services
{
    public class FriendService
    {
        private readonly HttpClient _httpClient;

        public FriendService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Friend>?> GetFriendsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Friend>>("api/v1/friends");
        }

        public async Task<bool> AddFriendAsync(Friend friend)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/friends", friend);
            return response.IsSuccessStatusCode;
        }
    }
}
