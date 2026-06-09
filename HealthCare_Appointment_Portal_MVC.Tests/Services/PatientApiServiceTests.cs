using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Models;
using HealthCare_Appointment_Portal_MVC.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HealthCare_Appointment_Portal_MVC.Tests.Services
{
    public class PatientApiServiceTests
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
                    new Uri(
                        "https://localhost/")
            };
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsPatients()
        {
            var patients =
                new List<PatientDto>
                {
                    new PatientDto
                    {
                        PatientId = 1
                    },
                    new PatientDto
                    {
                        PatientId = 2
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                patients),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new PatientApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAllPatientsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsPatient()
        {
            var patient =
                new PatientDto
                {
                    PatientId = 1
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                patient),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new PatientApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetPatientByIdAsync(
                        1);

            Assert.Equal(
                1,
                result.PatientId);
        }

        [Fact]
        public async Task GetPatientByEmailAsync_ReturnsPatient()
        {
            var patient =
                new PatientDto
                {
                    PatientId = 10
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                patient),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new PatientApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetPatientByEmailAsync(
                        "test@gmail.com");

            Assert.Equal(
                10,
                result.PatientId);
        }

        [Fact]
        public async Task GetPatientsByInsuranceStatusAsync_ReturnsPatients()
        {
            var patients =
                new List<PatientDto>
                {
                    new PatientDto
                    {
                        PatientId = 1
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                patients),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new PatientApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetPatientsByInsuranceStatusAsync(
                        InsuranceStatus.Active);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task CreatePatientAsync_ReturnsPatientId()
        {
            var created =
                new PatientDto
                {
                    PatientId = 5
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.Created)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                created),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new PatientApiService(
                    CreateClient(response));

            var result =
                await service
                    .CreatePatientAsync(
                        new CreatePatientDto());

            Assert.Equal(
                5,
                result);
        }

        [Fact]
        public async Task UpdatePatientAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new PatientApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.UpdatePatientAsync(
                        1,
                        new UpdatePatientDto()));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeletePatientAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new PatientApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.DeletePatientAsync(
                        1));

            Assert.Null(exception);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenApiReturnsError_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Patient not found"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NotFound)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                error),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new PatientApiService(
                    CreateClient(response));

            var exception =
                await Assert.ThrowsAsync<
                    HttpRequestException>(
                    () =>
                        service.GetPatientByIdAsync(
                            999));

            Assert.Equal(
                "Patient not found",
                exception.Message);
        }
    }
}