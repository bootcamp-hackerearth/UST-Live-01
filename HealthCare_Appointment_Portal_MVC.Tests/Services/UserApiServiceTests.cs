using HealthCare_Appointment_Portal.DTOs.UserDtos;
using HealthCare_Appointment_Portal_MVC.Models;
using HealthCare_Appointment_Portal_MVC.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;


namespace HealthCare_Appointment_Portal_MVC.Tests.Services
{
    public class UserApiServiceTests
    {
        private static HttpClient CreateClient(
            HttpResponseMessage response)
        {
            var handler =
                new Mock<HttpMessageHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return new HttpClient(
                handler.Object)
            {
                BaseAddress =
                    new Uri("https://localhost/")
            };
        }

        private static StringContent JsonContent(
            object data)
        {
            return new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");
        }

        [Fact]
        public async Task GetUserByCodeAsync_ReturnsUser()
        {
            var user =
                new UserDto
                {
                    UserId = 1,
                    UserCode = "P001"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(user)
                };

            var service =
                new UserApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetUserByCodeAsync(
                        "P001");

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.UserId);

            Assert.Equal(
                "P001",
                result.UserCode);
        }

        [Fact]
        public async Task GetUserByCodeAsync_WhenUserNotFound_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "User not found"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NotFound)
                {
                    Content =
                        JsonContent(error)
                };

            var service =
                new UserApiService(
                    CreateClient(response));

            var exception =
                await Assert.ThrowsAsync<HttpRequestException>(
                    () =>
                        service.GetUserByCodeAsync(
                            "INVALID"));

            Assert.Equal(
                "User not found",
                exception.Message);
        }

        [Fact]
        public async Task GetUserByCodeAsync_WhenServerError_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Internal Server Error"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.InternalServerError)
                {
                    Content =
                        JsonContent(error)
                };

            var service =
                new UserApiService(
                    CreateClient(response));

            var exception =
                await Assert.ThrowsAsync<HttpRequestException>(
                    () =>
                        service.GetUserByCodeAsync(
                            "P001"));

            Assert.Equal(
                "Internal Server Error",
                exception.Message);
        }
    }
}