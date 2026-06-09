using FluentAssertions;
using HealthcareMvcApiTests.Fakes;
using HealthcareWeb.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareMvcApiTests.Services.Web
{
    public class AppointmentApiServiceQueryTests
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
        public async Task GetUpcomingByPatientAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                { ""AppointmentId"": 1, ""PatientId"": 10, ""DoctorId"": 20, ""Status"": 1 },
                { ""AppointmentId"": 2, ""PatientId"": 10, ""DoctorId"": 30, ""Status"": 1 }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetUpcomingByPatientAsync(10);

            // Assert
            result.Should().HaveCount(2);
            result[0].AppointmentId.Should().Be(1);
            result[0].PatientId.Should().Be(10);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/patient/10/upcoming");
        }

        [Fact]
        public async Task GetUpcomingByDoctorAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                { ""AppointmentId"": 3, ""PatientId"": 10, ""DoctorId"": 20, ""Status"": 1 },
                { ""AppointmentId"": 4, ""PatientId"": 11, ""DoctorId"": 20, ""Status"": 1 }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetUpcomingByDoctorAsync(20);

            // Assert
            result.Should().HaveCount(2);
            result[0].DoctorId.Should().Be(20);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/doctor/20/upcoming");
        }

        [Fact]
        public async Task GetCancelledByPatientAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""AppointmentId"": 5,
                    ""PatientId"": 10,
                    ""DoctorId"": 20,
                    ""Status"": 4,
                    ""CancellationReason"": ""Patient unavailable""
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetCancelledByPatientAsync(10);

            // Assert
            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(5);
            result[0].CancellationReason.Should().Be("Patient unavailable");
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/patient/10/cancelled");
        }

        [Fact]
        public async Task GetCancelledByDoctorAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""AppointmentId"": 6,
                    ""PatientId"": 10,
                    ""DoctorId"": 20,
                    ""Status"": 5,
                    ""CancellationReason"": ""Doctor unavailable""
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetCancelledByDoctorAsync(20);

            // Assert
            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(6);
            result[0].CancellationReason.Should().Be("Doctor unavailable");
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/doctor/20/cancelled");
        }

        [Fact]
        public async Task GetUpcomingByPatientAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var errorJson = @"{ ""message"": ""No upcoming appointments found."" }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound, errorJson),
                out _);

            // Act
            Func<Task> action = async () => await service.GetUpcomingByPatientAsync(10);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("No upcoming appointments found.");
        }

        [Fact]
        public async Task GetUpcomingByDoctorAsync_ThrowsException_WithRawMessage_WhenResponseIsNotJson()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.InternalServerError, "Internal Server Error"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetUpcomingByDoctorAsync(20);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Internal Server Error");
        }
    }
}