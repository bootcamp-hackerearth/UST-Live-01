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
    public class HealthRecordApiServiceTests
    {
        private const string BaseUrl = "https://localhost:44364/api/";

        private static HealthRecordApiService CreateService(
            HttpResponseMessage response,
            out FakeHttpMessageHandler handler)
        {
            handler = new FakeHttpMessageHandler(response);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return new HealthRecordApiService(httpClient);
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
        public async Task GetAllAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""HealthRecordId"": 1,
                    ""PatientId"": 10,
                    ""DoctorId"": 20,
                    ""AppointmentId"": 5,
                    ""Diagnosis"": ""Flu"",
                    ""Prescription"": ""Rest"",
                    ""Notes"": ""Monitor""
                },
                {
                    ""HealthRecordId"": 2,
                    ""PatientId"": 11,
                    ""DoctorId"": 21,
                    ""AppointmentId"": 6,
                    ""Diagnosis"": ""Cold"",
                    ""Prescription"": ""Vitamins"",
                    ""Notes"": """"
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result[0].HealthRecordId.Should().Be(1);
            result[0].Diagnosis.Should().Be("Flu");
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords");
        }

        [Fact]
        public async Task GetAllAsync_ThrowsException_WithErrorMessage_OnFailure()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.InternalServerError, "Server error"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetAllAsync();

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Server error");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"{
                ""HealthRecordId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""AppointmentId"": 5,
                ""Diagnosis"": ""Flu"",
                ""Prescription"": ""Rest"",
                ""Notes"": ""Monitor""
            }";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            result.HealthRecordId.Should().Be(1);
            result.Diagnosis.Should().Be("Flu");
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords/1");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithErrorMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound, "Health record not found."),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Health record not found.");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithFallbackMessage_WhenErrorBodyIsEmpty()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var service = CreateService(response, out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(1);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("API request failed.");
        }

        [Fact]
        public async Task GetByPatientAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""HealthRecordId"": 1,
                    ""PatientId"": 10,
                    ""DoctorId"": 20,
                    ""AppointmentId"": 5,
                    ""Diagnosis"": ""Flu""
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetByPatientAsync(10);

            // Assert
            result.Should().HaveCount(1);
            result[0].PatientId.Should().Be(10);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords/patient/10");
        }

        [Fact]
        public async Task GetByPatientAsync_ThrowsException_WithErrorMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound, "No records found for patient."),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByPatientAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("No records found for patient.");
        }

        [Fact]
        public async Task GetByDoctorAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""HealthRecordId"": 2,
                    ""PatientId"": 11,
                    ""DoctorId"": 20,
                    ""AppointmentId"": 6,
                    ""Diagnosis"": ""Cold""
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetByDoctorAsync(20);

            // Assert
            result.Should().HaveCount(1);
            result[0].DoctorId.Should().Be(20);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords/doctor/20");
        }

        [Fact]
        public async Task GetByAppointmentAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""HealthRecordId"": 3,
                    ""PatientId"": 10,
                    ""DoctorId"": 20,
                    ""AppointmentId"": 5,
                    ""Diagnosis"": ""Migraine""
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetByAppointmentAsync(5);

            // Assert
            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(5);
            result[0].Diagnosis.Should().Be("Migraine");
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords/appointment/5");
        }

        [Fact]
        public async Task AddAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var responseJson = @"{
                ""HealthRecordId"": 10,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""AppointmentId"": 5,
                ""Diagnosis"": ""Flu"",
                ""Prescription"": ""Rest"",
                ""Notes"": ""Follow up in a week""
            }";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, responseJson), out var handler);

            var dto = new AddHealthRecordDto
            {
                AppointmentId = 5,
                Diagnosis = "Flu",
                Prescription = "Rest",
                Notes = "Follow up in a week"
            };

            // Act
            var result = await service.AddAsync(dto);

            // Assert
            result.HealthRecordId.Should().Be(10);
            result.Diagnosis.Should().Be("Flu");
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["AppointmentId"].Value<int>().Should().Be(5);
            body["Diagnosis"].Value<string>().Should().Be("Flu");
            body["Prescription"].Value<string>().Should().Be("Rest");
            body["Notes"].Value<string>().Should().Be("Follow up in a week");
        }

        [Fact]
        public async Task AddAsync_ThrowsException_WithErrorMessage_OnBadRequest()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest, "Invalid appointment."),
                out _);

            var dto = new AddHealthRecordDto
            {
                AppointmentId = 5,
                Diagnosis = "Flu",
                Prescription = "Rest"
            };

            // Act
            Func<Task> action = async () => await service.AddAsync(dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid appointment.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsDto_AndPutsToCorrectUrl()
        {
            // Arrange
            var responseJson = @"{
                ""HealthRecordId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""AppointmentId"": 5,
                ""Diagnosis"": ""Updated Flu"",
                ""Prescription"": ""Updated Rest"",
                ""Notes"": ""Updated notes""
            }";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, responseJson), out var handler);

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Flu",
                Prescription = "Updated Rest",
                Notes = "Updated notes"
            };

            // Act
            var result = await service.UpdateAsync(1, dto);

            // Assert
            result.HealthRecordId.Should().Be(1);
            result.Diagnosis.Should().Be("Updated Flu");
            handler.Request.Method.Should().Be(HttpMethod.Put);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords/1");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["Diagnosis"].Value<string>().Should().Be("Updated Flu");
            body["Prescription"].Value<string>().Should().Be("Updated Rest");
            body["Notes"].Value<string>().Should().Be("Updated notes");
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WithErrorMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound, "Health record not found."),
                out _);

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Flu",
                Prescription = "Updated Rest"
            };

            // Act
            Func<Task> action = async () => await service.UpdateAsync(99, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Health record not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsDto_AndSendsDeleteToCorrectUrl()
        {
            // Arrange
            var responseJson = @"{
                ""HealthRecordId"": 1,
                ""PatientId"": 10,
                ""DoctorId"": 20,
                ""AppointmentId"": 5,
                ""Diagnosis"": ""Flu""
            }";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, responseJson), out var handler);

            // Act
            var result = await service.DeleteAsync(1);

            // Assert
            result.HealthRecordId.Should().Be(1);
            handler.Request.Method.Should().Be(HttpMethod.Delete);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "healthrecords/1");
            handler.Request.Content.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WithErrorMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound, "Health record not found."),
                out _);

            // Act
            Func<Task> action = async () => await service.DeleteAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Health record not found.");
        }
    }
}