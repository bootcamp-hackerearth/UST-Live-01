using FluentAssertions;
using HealthcareMvcApiTests.Fakes;
using HealthcareWeb.Services;
using Newtonsoft.Json.Linq;
using SharedClasses.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareMvcApiTests.Services.Web
{
    public class AppointmentApiServiceTests
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

        private static string AppointmentJson()
        {
            return @"{
                ""appointmentId"": 1,
                ""patientId"": 10,
                ""doctorId"": 20,
                ""scheduledDate"": ""2026-06-10T00:00:00"",
                ""slotNumber"": 1,
                ""status"": 0,
                ""cancellationReason"": """"
            }";
        }

        private static string AppointmentListJson()
        {
            return @"[
                {
                    ""appointmentId"": 1,
                    ""patientId"": 10,
                    ""doctorId"": 20,
                    ""scheduledDate"": ""2026-06-10T00:00:00"",
                    ""slotNumber"": 1,
                    ""status"": 0,
                    ""cancellationReason"": """"
                },
                {
                    ""appointmentId"": 2,
                    ""patientId"": 11,
                    ""doctorId"": 21,
                    ""scheduledDate"": ""2026-06-11T00:00:00"",
                    ""slotNumber"": 2,
                    ""status"": 1,
                    ""cancellationReason"": """"
                }
            ]";
        }

        [Fact]
        public async Task GetAllAsync_WhenAppointmentsExist_ShouldReturnAppointmentList()
        {
            // Arrange
            var response = JsonResponse(HttpStatusCode.OK, AppointmentListJson());
            var service = CreateService(response, out FakeHttpMessageHandler handler);

            // Act
            List<AppointmentDto> result = await service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].AppointmentId.Should().Be(1);
            result[1].AppointmentId.Should().Be(2);

            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments");
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentExists_ShouldReturnAppointmentDto()
        {
            // Arrange
            var response = JsonResponse(HttpStatusCode.OK, AppointmentJson());
            var service = CreateService(response, out FakeHttpMessageHandler handler);

            // Act
            AppointmentDto result = await service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.PatientId.Should().Be(10);
            result.DoctorId.Should().Be(20);

            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1");
        }

        [Fact]
        public async Task GetByPatientAsync_WhenAppointmentsExist_ShouldCallPatientEndpoint()
        {
            // Arrange
            var response = JsonResponse(HttpStatusCode.OK, AppointmentListJson());
            var service = CreateService(response, out FakeHttpMessageHandler handler);

            // Act
            List<AppointmentDto> result = await service.GetByPatientAsync(10);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/patient/10");
        }

        [Fact]
        public async Task GetByDoctorAsync_WhenAppointmentsExist_ShouldCallDoctorEndpoint()
        {
            // Arrange
            var response = JsonResponse(HttpStatusCode.OK, AppointmentListJson());
            var service = CreateService(response, out FakeHttpMessageHandler handler);

            // Act
            List<AppointmentDto> result = await service.GetByDoctorAsync(20);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/doctor/20");
        }

        [Fact]
        public async Task BookAsync_WhenApiCreatesAppointment_ShouldSendPostAndReturnAppointmentDto()
        {
            // Arrange
            var dto = new BookAppointmentDto
            {
                PatientId = 10,
                DoctorId = 20,
                ScheduledDate = new DateTime(2026, 6, 10)
            };

            var response = JsonResponse(HttpStatusCode.Created, AppointmentJson());
            var service = CreateService(response, out FakeHttpMessageHandler handler);

            // Act
            AppointmentDto result = await service.BookAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.PatientId.Should().Be(10);
            result.DoctorId.Should().Be(20);

            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/book");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);

            body["PatientId"].Value<int>().Should().Be(10);
            body["DoctorId"].Value<int>().Should().Be(20);
            body["ScheduledDate"].Should().NotBeNull();

            body["SlotNumber"].Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenApiUpdatesAppointment_ShouldSendPutAndReturnAppointmentDto()
        {
            // Arrange
            var dto = new UpdateAppointmentDto
            {
                PatientId = 10,
                DoctorId = 20,
                ScheduledDate = new DateTime(2026, 6, 10),
                SlotNumber = 2
            };

            string json = @"{
                ""appointmentId"": 1,
                ""patientId"": 10,
                ""doctorId"": 20,
                ""scheduledDate"": ""2026-06-10T00:00:00"",
                ""slotNumber"": 2,
                ""status"": 0,
                ""cancellationReason"": """"
            }";

            var response = JsonResponse(HttpStatusCode.OK, json);
            var service = CreateService(response, out FakeHttpMessageHandler handler);

            // Act
            AppointmentDto result = await service.UpdateAsync(1, dto);

            // Assert
            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.SlotNumber.Should().Be(2);

            handler.Request.Method.Should().Be(HttpMethod.Put);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "appointments/1");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);

            body["PatientId"].Value<int>().Should().Be(10);
            body["DoctorId"].Value<int>().Should().Be(20);
            body["SlotNumber"].Value<int>().Should().Be(2);
        }

        [Fact]
        public async Task GetByIdAsync_WhenApiReturnsNotFound_ShouldThrowCleanMessage()
        {
            // Arrange
            string json = @"{
                ""message"": ""Appointment with ID 999 was not found.""
            }";

            var response = JsonResponse(HttpStatusCode.NotFound, json);
            var service = CreateService(response, out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(999);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment with ID 999 was not found.");
        }

        [Fact]
        public async Task BookAsync_WhenApiReturnsBadRequest_ShouldThrowCleanMessage()
        {
            // Arrange
            var dto = new BookAppointmentDto
            {
                PatientId = 10,
                DoctorId = 20,
                ScheduledDate = DateTime.Today.AddDays(-1)
            };

            string json = @"{
                ""message"": ""Appointment date cannot be in the past.""
            }";

            var response = JsonResponse(HttpStatusCode.BadRequest, json);
            var service = CreateService(response, out _);

            // Act
            Func<Task> action = async () => await service.BookAsync(dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment date cannot be in the past.");
        }
    }
}