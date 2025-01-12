using Frontend.Entities.Profile.Model;
using Frontend.Features.Services.Interfaces;

namespace Frontend.Features.Services;

public class ProfileService : IProfileService
{
    private readonly HttpClient _httpClient;

    public ProfileService()
    {
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };

        _httpClient = new HttpClient(httpClientHandler);
    }

    public async Task<UserProfileResponse> GetUserProfile(GetUserProfileRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        string host = Environment.GetEnvironmentVariable("PROFILE_HOST") ?? "localhost";
        string url = $"http://{host}:8088/api/profile/getuser";

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync(url, request);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while sending the HTTP request.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"HTTP Request failed with status code {response.StatusCode}: {errorContent}");
        }

        try
        {
            var userProfileResponse = await response.Content.ReadFromJsonAsync<UserProfileResponse>();

            if (userProfileResponse == null)
            {
                throw new Exception("Response content is null or cannot be deserialized.");
            }

            return userProfileResponse;
        }
        catch (Exception ex)
        {
            throw new Exception("Error while deserializing the HTTP response.", ex);
        }
    }
}