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

        public async Task<int> GetFriendsCountAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>("api/v1/friends/count");
        }

        public async Task<bool> AddFriendAsync(Friend friend)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/friends", friend);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFriendsAsync()
        {
            try
            {
                await _httpClient.DeleteAsync("api/v1/friends");
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
