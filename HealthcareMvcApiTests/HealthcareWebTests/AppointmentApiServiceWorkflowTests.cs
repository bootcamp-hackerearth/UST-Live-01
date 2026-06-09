using FluentAssertions;
using HealthcareMvcApiTests.Fakes;
using HealthcareWeb.Services;
using Newtonsoft.Json.Linq;
using SharedClasses.Dtos;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareMvcApiTests.Services.Web
{
    public class AppointmentApiServiceWorkflowActionTests
    {
        private const string BaseUrl = "https://localhost:44364/api/";

        private static AppointmentApiService CreateService(
            HttpResponseMessage response,
            out FakeHttpMessageHandler handler)
        {
            handler = new FakeHttpMessageHandler(response);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return new AppointmentApiService(httpClient);
        }

        private static HttpResponseMessage JsonResponse(
            HttpStatusCode statusCode,
            string json)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        [Fact]
        public async Task ConfirmAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var appointmentJson = @"{
                ""AppointmentId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""ScheduledDate"": ""2025-01-01T09:00:00"",
                ""SlotNumber"": 1,
                ""Status"": 1
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, appointmentJson),
                out var handler);

            var dto = new ConfirmAppointmentDto { DoctorId = 20 };

            // Act
            var result = await service.ConfirmAsync(1, dto);

            // Assert
            result.AppointmentId.Should().Be(1);
            result.DoctorId.Should().Be(20);
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1/confirm");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["DoctorId"].Value<int>().Should().Be(20);
        }

        [Fact]
        public async Task ConfirmAsync_ThrowsException_WithCleanMessage_OnBadRequest()
        {
            // Arrange
            var errorJson = @"{ ""message"": ""Appointment cannot be confirmed."" }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest, errorJson),
                out _);

            var dto = new ConfirmAppointmentDto { DoctorId = 20 };

            // Act
            Func<Task> action = async () => await service.ConfirmAsync(1, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment cannot be confirmed.");
        }

        [Fact]
        public async Task CompleteAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var appointmentJson = @"{
                ""AppointmentId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""ScheduledDate"": ""2025-01-01T09:00:00"",
                ""SlotNumber"": 1,
                ""Status"": 3
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, appointmentJson),
                out var handler);

            var dto = new CompleteAppointmentDto { DoctorId = 20 };

            // Act
            var result = await service.CompleteAsync(1, dto);

            // Assert
            result.AppointmentId.Should().Be(1);
            result.DoctorId.Should().Be(20);
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1/complete");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["DoctorId"].Value<int>().Should().Be(20);
        }

        [Fact]
        public async Task CompleteAsync_ThrowsException_WithCleanMessage_OnBadRequest()
        {
            // Arrange
            var errorJson = @"{ ""message"": ""Appointment cannot be completed."" }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest, errorJson),
                out _);

            var dto = new CompleteAppointmentDto { DoctorId = 20 };

            // Act
            Func<Task> action = async () => await service.CompleteAsync(1, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment cannot be completed.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsDto_AndSendsDeleteToCorrectUrl()
        {
            // Arrange
            var appointmentJson = @"{
                ""AppointmentId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""ScheduledDate"": ""2025-01-01T09:00:00"",
                ""SlotNumber"": 1,
                ""Status"": 0
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, appointmentJson),
                out var handler);

            // Act
            var result = await service.DeleteAsync(1);

            // Assert
            result.AppointmentId.Should().Be(1);
            handler.Request.Method.Should().Be(HttpMethod.Delete);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1");
            handler.Request.Content.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var errorJson = @"{ ""message"": ""Appointment not found."" }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound, errorJson),
                out _);

            // Act
            Func<Task> action = async () => await service.DeleteAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment not found.");
        }
    }
}