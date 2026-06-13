using HealthcareWeb.Services;
using Newtonsoft.Json;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareWeb.Tests.Services
{
    public class AppointmentApiServiceTests
    {
        [Theory]
        [MemberData(nameof(GetListEndpointCases))]
        public async Task ListMethods_WhenApiReturnsSuccess_ShouldReturnAppointments(
            string expectedUrl,
            Func<AppointmentApiService, Task<List<AppointmentDto>>> action)
        {
            // Arrange
            var expectedAppointments = new List<AppointmentDto>
            {
                CreateAppointmentDto()
            };

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, expectedAppointments));

            var service = CreateService(handler);

            // Act
            List<AppointmentDto> result = await action(service);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(10, result[0].AppointmentId);
            Assert.Equal(expectedUrl, handler.LastRequest.RequestUri.ToString());
            Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        }

        public static IEnumerable<object[]> GetListEndpointCases()
        {
            yield return new object[]
            {
                "http://localhost/api/appointments",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetAllAsync())
            };

            yield return new object[]
            {
                "http://localhost/api/appointments/patient/1",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetByPatientAsync(1))
            };

            yield return new object[]
            {
                "http://localhost/api/appointments/doctor/2",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetByDoctorAsync(2))
            };

            yield return new object[]
            {
                "http://localhost/api/appointments/patient/1/upcoming",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetUpcomingByPatientAsync(1))
            };

            yield return new object[]
            {
                "http://localhost/api/appointments/doctor/2/upcoming",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetUpcomingByDoctorAsync(2))
            };

            yield return new object[]
            {
                "http://localhost/api/appointments/patient/1/cancelled",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetCancelledByPatientAsync(1))
            };

            yield return new object[]
            {
                "http://localhost/api/appointments/doctor/2/cancelled",
                new Func<AppointmentApiService, Task<List<AppointmentDto>>>(s => s.GetCancelledByDoctorAsync(2))
            };
        }

        [Fact]
        public async Task GetByIdAsync_WhenApiReturnsSuccess_ShouldReturnAppointment()
        {
            // Arrange
            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.GetByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal("http://localhost/api/appointments/10", handler.LastRequest.RequestUri.ToString());
            Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        }

        [Fact]
        public async Task BookAsync_WhenApiReturnsSuccess_ShouldPostJsonAndReturnAppointment()
        {
            // Arrange
            var dto = new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                SlotNumber = 4
            };

            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.BookAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/book", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"PatientId\":1", handler.LastRequestBody);
            Assert.Contains("\"DoctorId\":2", handler.LastRequestBody);
            Assert.Contains("\"SlotNumber\":4", handler.LastRequestBody);
        }

        [Fact]
        public async Task UpdateAsync_WhenApiReturnsSuccess_ShouldPutJsonAndReturnAppointment()
        {
            // Arrange
            var dto = new UpdateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                SlotNumber = 5
            };

            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.UpdateAsync(10, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Put, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/10", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"SlotNumber\":5", handler.LastRequestBody);
        }

        [Fact]
        public async Task DeleteAsync_WhenApiReturnsSuccess_ShouldDeleteAndReturnAppointment()
        {
            // Arrange
            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.DeleteAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/10", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task ConfirmAsync_WhenApiReturnsSuccess_ShouldPostJsonAndReturnAppointment()
        {
            // Arrange
            var dto = new ConfirmAppointmentDto
            {
                DoctorId = 2
            };

            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.ConfirmAsync(10, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/10/confirm", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"DoctorId\":2", handler.LastRequestBody);
        }

        [Fact]
        public async Task CancelByPatientAsync_WhenApiReturnsSuccess_ShouldPostJsonAndReturnAppointment()
        {
            // Arrange
            var dto = new CancelByPatientDto
            {
                PatientId = 1,
                Reason = "Not available"
            };

            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.CancelByPatientAsync(10, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/10/cancel-by-patient", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"PatientId\":1", handler.LastRequestBody);
            Assert.Contains("Not available", handler.LastRequestBody);
        }

        [Fact]
        public async Task CancelByDoctorAsync_WhenApiReturnsSuccess_ShouldPostJsonAndReturnAppointment()
        {
            // Arrange
            var dto = new CancelByDoctorDto
            {
                DoctorId = 2,
                Reason = "Emergency"
            };

            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.CancelByDoctorAsync(10, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/10/cancel-by-doctor", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"DoctorId\":2", handler.LastRequestBody);
            Assert.Contains("Emergency", handler.LastRequestBody);
        }

        [Fact]
        public async Task CompleteAsync_WhenApiReturnsSuccess_ShouldPostJsonAndReturnAppointment()
        {
            // Arrange
            var dto = new CompleteAppointmentDto
            {
                DoctorId = 2
            };

            var appointment = CreateAppointmentDto();

            var handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, appointment));

            var service = CreateService(handler);

            // Act
            AppointmentDto result = await service.CompleteAsync(10, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/appointments/10/complete", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"DoctorId\":2", handler.LastRequestBody);
        }

        [Theory]
        [InlineData("{\"message\":\"Selected appointment slot is already booked.\"}", "Selected appointment slot is already booked.")]
        [InlineData("{\"Message\":\"Appointment date cannot be in the past.\"}", "Appointment date cannot be in the past.")]
        [InlineData("{\"exceptionMessage\":\"Doctor is inactive.\"}", "Doctor is inactive.")]
        [InlineData("{\"ExceptionMessage\":\"Patient not found.\"}", "Patient not found.")]
        [InlineData("Plain text error", "Plain text error")]
        [InlineData("", "API request failed.")]
        public async Task GetAllAsync_WhenApiReturnsError_ShouldThrowExtractedMessage(
            string responseContent,
            string expectedMessage)
        {
            // Arrange
            var handler = new FakeHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var service = CreateService(handler);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() =>
                service.GetAllAsync());

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_WhenApiReturnsModelState_ShouldThrowCombinedModelStateErrors()
        {
            // Arrange
            string responseContent =
                "{\"modelState\":{\"FullName\":[\"Name is required.\"],\"PhoneNumber\":[\"Phone number must contain exactly 10 digits.\"]}}";

            var handler = new FakeHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var service = CreateService(handler);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() =>
                service.GetAllAsync());

            // Assert
            Assert.Contains("Name is required.", exception.Message);
            Assert.Contains("Phone number must contain exactly 10 digits.", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_WhenApiReturnsUppercaseModelState_ShouldThrowCombinedModelStateErrors()
        {
            // Arrange
            string responseContent =
                "{\"ModelState\":{\"ScheduledDate\":[\"Appointment date is required.\"],\"SlotNumber\":[\"Please select a valid appointment slot.\"]}}";

            var handler = new FakeHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var service = CreateService(handler);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() =>
                service.GetAllAsync());

            // Assert
            Assert.Contains("Appointment date is required.", exception.Message);
            Assert.Contains("Please select a valid appointment slot.", exception.Message);
        }

        private static AppointmentApiService CreateService(FakeHttpMessageHandler handler)
        {
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost/api/")
            };

            return new AppointmentApiService(httpClient);
        }

        private static HttpResponseMessage CreateJsonResponse<T>(
            HttpStatusCode statusCode,
            T value)
        {
            string json = JsonConvert.SerializeObject(value);

            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static AppointmentDto CreateAppointmentDto()
        {
            return new AppointmentDto
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                SlotNumber = 4,
                Status = AppointmentStatus.Pending,
                CancellationReason = string.Empty
            };
        }

        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public HttpRequestMessage LastRequest { get; private set; }

            public string LastRequestBody { get; private set; }

            public FakeHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                LastRequest = request;

                if (request.Content != null)
                {
                    LastRequestBody = await request.Content.ReadAsStringAsync();
                }
                else
                {
                    LastRequestBody = string.Empty;
                }

                return _response;
            }
        }
    }
}