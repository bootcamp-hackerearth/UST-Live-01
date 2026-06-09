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
    public class AppointmentApiServiceCancellationTests
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
        public async Task CancelByPatientAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var appointmentJson = @"{
                ""AppointmentId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""ScheduledDate"": ""2025-01-01T09:00:00"",
                ""SlotNumber"": 1,
                ""Status"": 4,
                ""CancellationReason"": ""Patient unavailable""
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, appointmentJson),
                out var handler);

            var dto = new CancelByPatientDto { PatientId = 10, Reason = "Patient unavailable" };

            // Act
            var result = await service.CancelByPatientAsync(1, dto);

            // Assert
            result.AppointmentId.Should().Be(1);
            result.PatientId.Should().Be(10);
            result.CancellationReason.Should().Be("Patient unavailable");
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1/cancel-by-patient");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["PatientId"].Value<int>().Should().Be(10);
            body["Reason"].Value<string>().Should().Be("Patient unavailable");
        }

        [Fact]
        public async Task CancelByPatientAsync_ThrowsException_WithCleanMessage_OnBadRequest()
        {
            // Arrange
            var errorJson = @"{ ""message"": ""Cancellation not allowed."" }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest, errorJson),
                out _);

            var dto = new CancelByPatientDto { PatientId = 10, Reason = "Patient unavailable" };

            // Act
            Func<Task> action = async () => await service.CancelByPatientAsync(1, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Cancellation not allowed.");
        }

        [Fact]
        public async Task CancelByDoctorAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var appointmentJson = @"{
                ""AppointmentId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""ScheduledDate"": ""2025-01-01T09:00:00"",
                ""SlotNumber"": 1,
                ""Status"": 5,
                ""CancellationReason"": ""Doctor unavailable""
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, appointmentJson),
                out var handler);

            var dto = new CancelByDoctorDto { DoctorId = 20, Reason = "Doctor unavailable" };

            // Act
            var result = await service.CancelByDoctorAsync(1, dto);

            // Assert
            result.AppointmentId.Should().Be(1);
            result.DoctorId.Should().Be(20);
            result.CancellationReason.Should().Be("Doctor unavailable");
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1/cancel-by-doctor");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["DoctorId"].Value<int>().Should().Be(20);
            body["Reason"].Value<string>().Should().Be("Doctor unavailable");
        }

        [Fact]
        public async Task CancelByDoctorAsync_ThrowsException_WithCleanMessage_OnBadRequest()
        {
            // Arrange
            var errorJson = @"{ ""message"": ""Doctor cancellation not allowed."" }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest, errorJson),
                out _);

            var dto = new CancelByDoctorDto { DoctorId = 20, Reason = "Doctor unavailable" };

            // Act
            Func<Task> action = async () => await service.CancelByDoctorAsync(1, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor cancellation not allowed.");
        }
    }
}