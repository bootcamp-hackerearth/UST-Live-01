using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal_MVC.Models;
using HealthCare_Appointment_Portal_MVC.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal_MVC.Tests.Services
{
    public class HealthRecordApiServiceTests
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
        public async Task GetAllHealthRecordsAsync_ReturnsRecords()
        {
            var records =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        RecordId = 1
                    },
                    new HealthRecordDto
                    {
                        RecordId = 2
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(records)
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAllHealthRecordsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_ReturnsRecord()
        {
            var record =
                new HealthRecordDto
                {
                    RecordId = 1
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(record)
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetHealthRecordByIdAsync(
                        1);

            Assert.Equal(
                1,
                result.RecordId);
        }

        [Fact]
        public async Task GetRecordsByPatientAsync_ReturnsRecords()
        {
            var records =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        RecordId = 1
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(records)
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetRecordsByPatientAsync(
                        1);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task GetRecordsByDoctorAsync_ReturnsRecords()
        {
            var records =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        RecordId = 1
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(records)
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetRecordsByDoctorAsync(
                        1);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task CreateHealthRecordAsync_ReturnsRecordId()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.Created)
                {
                    Content =
                        JsonContent(
                            new
                            {
                                RecordId = 10
                            })
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var result =
                await service
                    .CreateHealthRecordAsync(
                        new CreateHealthRecordDto());

            Assert.Equal(
                10,
                result);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.UpdateHealthRecordAsync(
                        1,
                        new UpdateHealthRecordDto()));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.DeleteHealthRecordAsync(
                        1));

            Assert.Null(exception);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenApiReturnsNotFound_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Record not found"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NotFound)
                {
                    Content =
                        JsonContent(error)
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var exception =
               await Assert.ThrowsAsync<HttpRequestException>(
                  () =>
                    service
                        .GetHealthRecordByIdAsync(
                            999));

            Assert.Equal(
                "Record not found",
                exception.Message);
        }

        [Fact]
        public async Task GetAllHealthRecordsAsync_WhenApiReturnsServerError_ThrowsException()
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
                new HealthRecordApiService(
                    CreateClient(response));

            var exception =
                await Assert.ThrowsAsync<HttpRequestException>(
                    () =>
                        service
                            .GetAllHealthRecordsAsync());

            Assert.Equal(
                "Internal Server Error",
                exception.Message);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenApiReturnsBadRequest_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Delete failed"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.BadRequest)
                {
                    Content =
                        JsonContent(error)
                };

            var service =
                new HealthRecordApiService(
                    CreateClient(response));

            var exception =
                await Assert.ThrowsAsync<HttpRequestException>(
                    () =>
                        service
                            .DeleteHealthRecordAsync(
                                1));

            Assert.Equal(
                "Delete failed",
                exception.Message);
        }
    }
}