using DNATestingSystem.Repository.TienDM.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Services
{
    public interface IApiService
    {
        Task<List<AppointmentsTienDm>> GetAppointmentsAsync(string token);
        Task<AppointmentsTienDm> GetAppointmentByIdAsync(int id, string token);
        Task<int> CreateAppointmentAsync(AppointmentsTienDm appointment, string token);
        Task<int> UpdateAppointmentAsync(AppointmentsTienDm appointment, string token);
        Task<bool> DeleteAppointmentAsync(int id, string token);
        Task<List<ServicesNhanVt>> GetServicesAsync(string token);
        Task<List<AppointmentStatusesTienDm>> GetAppointmentStatusesAsync(string token);
        Task<List<SystemUserAccount>> GetUserAccountsAsync(string token);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = "https://localhost:8080/api"; // Backend API base URL
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<AppointmentsTienDm>> GetAppointmentsAsync(string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/AppointmentsTienDM");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<AppointmentsTienDm>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<AppointmentsTienDm>();
            }
            return new List<AppointmentsTienDm>();
        }

        public async Task<AppointmentsTienDm> GetAppointmentByIdAsync(int id, string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/AppointmentsTienDM/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AppointmentsTienDm>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new AppointmentsTienDm();
            }
            return new AppointmentsTienDm();
        }

        public async Task<int> CreateAppointmentAsync(AppointmentsTienDm appointment, string token)
        {
            SetAuthorizationHeader(token);
            var json = JsonSerializer.Serialize(appointment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/AppointmentsTienDM", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (int.TryParse(responseContent, out int id))
                {
                    return id;
                }
            }
            return 0;
        }

        public async Task<int> UpdateAppointmentAsync(AppointmentsTienDm appointment, string token)
        {
            SetAuthorizationHeader(token);
            var json = JsonSerializer.Serialize(appointment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{_baseUrl}/AppointmentsTienDM/{appointment.AppointmentsTienDmid}", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (int.TryParse(responseContent, out int id))
                {
                    return id;
                }
            }
            return 0;
        }

        public async Task<bool> DeleteAppointmentAsync(int id, string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/AppointmentsTienDM/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (bool.TryParse(responseContent, out bool result))
                {
                    return result;
                }
            }
            return false;
        }

        // Helper methods to get related data
        public async Task<List<ServicesNhanVt>> GetServicesAsync(string token)
        {
            // Note: Assuming there's a Services endpoint in the backend
            // If not available, this method would need to be adjusted
            return new List<ServicesNhanVt>();
        }

        public async Task<List<AppointmentStatusesTienDm>> GetAppointmentStatusesAsync(string token)
        {
            // Note: Assuming there's an AppointmentStatuses endpoint in the backend
            // If not available, this method would need to be adjusted
            return new List<AppointmentStatusesTienDm>();
        }

        public async Task<List<SystemUserAccount>> GetUserAccountsAsync(string token)
        {
            // Note: Assuming there's a UserAccounts endpoint in the backend
            // If not available, this method would need to be adjusted
            return new List<SystemUserAccount>();
        }
    }
}
