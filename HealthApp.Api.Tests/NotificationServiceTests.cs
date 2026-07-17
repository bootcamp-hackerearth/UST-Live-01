using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services;

public class NotificationServiceTests
{
    private readonly Mock<INotificationRepository> _repository = new();
    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        _service = new NotificationService(_repository.Object);
    }

    [Fact]
    public async Task CreateNotificationAsync_WhenDtoIsNull_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.CreateNotificationAsync(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateNotificationAsync_WhenRecipientIsMissing_ShouldThrow(
        string? recipientUserId)
    {
        var dto = CreateDto();
        dto.RecipientUserId = recipientUserId!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.CreateNotificationAsync(dto));
    }

    [Fact]
    public async Task CreateNotificationAsync_WithValidData_ShouldAddAndSave()
    {
        var dto = CreateDto();
        Notification? capturedNotification = null;

        _repository
            .Setup(repository => repository.AddAsync(
                It.IsAny<Notification>(),
                It.IsAny<CancellationToken>()))
            .Callback<Notification, CancellationToken>(
                (notification, _) => capturedNotification = notification)
            .Returns(Task.CompletedTask);
        _repository
            .Setup(repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _service.CreateNotificationAsync(dto);

        Assert.NotNull(capturedNotification);
        Assert.Equal(dto.RecipientUserId, capturedNotification.RecipientUserId);
        Assert.Equal(dto.NotificationType, capturedNotification.NotificationType);
        Assert.Equal(dto.Title, capturedNotification.Title);
        Assert.Equal(dto.Message, capturedNotification.Message);
        Assert.Equal(dto.RelatedEntityId, capturedNotification.RelatedEntityId);
        Assert.Equal(dto.RelatedEntityType, capturedNotification.RelatedEntityType);
        Assert.False(capturedNotification.IsRead);
        _repository.Verify(
            repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetUnreadNotifications_WhenUserIdIsMissing_ShouldThrow(
        string? recipientUserId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetUnreadDoctorLeaveNotificationsAsync(
                recipientUserId!));
    }

    [Fact]
    public async Task GetUnreadNotifications_ShouldMapRepositoryResults()
    {
        var createdAt = DateTime.UtcNow;
        _repository
            .Setup(repository => repository.GetUnreadDoctorLeaveAsync(
                "user-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>
            {
                new()
                {
                    NotificationId = 1,
                    RecipientUserId = "user-1",
                    NotificationType = "DoctorLeave",
                    Title = "Leave",
                    Message = "Appointment cancelled",
                    RelatedEntityId = 10,
                    RelatedEntityType = "Appointment",
                    IsRead = false,
                    CreatedAt = createdAt
                }
            });

        var result = await _service
            .GetUnreadDoctorLeaveNotificationsAsync("user-1");

        var dto = Assert.Single(result);
        Assert.Equal(1, dto.NotificationId);
        Assert.Equal("DoctorLeave", dto.NotificationType);
        Assert.Equal("Leave", dto.Title);
        Assert.Equal("Appointment cancelled", dto.Message);
        Assert.Equal(10, dto.RelatedEntityId);
        Assert.Equal("Appointment", dto.RelatedEntityType);
        Assert.False(dto.IsRead);
        Assert.Equal(createdAt, dto.CreatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task AcknowledgeNotificationAsync_WhenIdIsInvalid_ShouldThrow(
        int notificationId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AcknowledgeNotificationAsync(
                notificationId,
                "user-1"));
    }

    [Fact]
    public async Task AcknowledgeNotificationAsync_WhenNotFound_ShouldThrow()
    {
        _repository
            .Setup(repository => repository.GetByIdAndRecipientAsync(
                99,
                "user-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.AcknowledgeNotificationAsync(99, "user-1"));
    }

    [Fact]
    public async Task AcknowledgeNotificationAsync_WhenUnread_ShouldMarkReadAndSave()
    {
        var notification = new Notification
        {
            NotificationId = 1,
            RecipientUserId = "user-1",
            IsRead = false
        };
        _repository
            .Setup(repository => repository.GetByIdAndRecipientAsync(
                1,
                "user-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);
        _repository
            .Setup(repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _service.AcknowledgeNotificationAsync(1, "user-1");

        Assert.True(notification.IsRead);
        _repository.Verify(
            repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task AcknowledgeNotificationAsync_WhenAlreadyRead_ShouldNotSave()
    {
        var notification = new Notification
        {
            NotificationId = 1,
            RecipientUserId = "user-1",
            IsRead = true
        };
        _repository
            .Setup(repository => repository.GetByIdAndRecipientAsync(
                1,
                "user-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        await _service.AcknowledgeNotificationAsync(1, "user-1");

        _repository.Verify(
            repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static CreateNotificationDto CreateDto() => new()
    {
        RecipientUserId = "user-1",
        NotificationType = "DoctorLeave",
        Title = "Leave",
        Message = "Appointment cancelled",
        RelatedEntityId = 10,
        RelatedEntityType = "Appointment"
    };
}
