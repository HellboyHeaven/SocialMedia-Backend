using Moq;
using Core;
using ServiceExeption.Exceptions;
using Microsoft.EntityFrameworkCore.Storage;
using Crypt = BCrypt.Net.BCrypt;

namespace Application.Tests;
public class RegisterUserHandlerTests
{
    private readonly Mock<IUserStore> _mockUserStore;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IDbContextTransaction> _mockTransaction;
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _mockUserStore = new Mock<IUserStore>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTransaction = new Mock<IDbContextTransaction>();

        _mockUnitOfWork.Setup(x => x.BeginTrasnactionAsync())
            .ReturnsAsync(_mockTransaction.Object);

        _handler = new RegisterUserHandler(_mockUserStore.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ValidRegistration_CreatesUser()
    {
        // Arrange
        var command = new RegisterUserCommand("newuser", "password");

        _mockUserStore.Setup(x => x.Create(It.IsAny<UserEntity>()))
            .ReturnsAsync(Guid.NewGuid());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockUserStore.Verify(x => x.Create(It.Is<UserEntity>(u =>
            u.Login == "newuser" && !string.IsNullOrEmpty(u.PasswordHash))), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        _mockTransaction.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once); // Should commit on success
    }

    [Fact]
    public async Task Handle_ExistingUser_ThrowsConflictException()
    {
        // Arrange
        var command = new RegisterUserCommand("existinguser", "password");

        _mockUserStore.Setup(x => x.Create(It.IsAny<UserEntity>()))
            .ThrowsAsync(new ConflictException("User already exist"));

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _mockTransaction.Verify(x => x.RollbackAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidRegistration_HashesPassword()
    {
        // Arrange
        var command = new RegisterUserCommand("newuser", "plainpassword");
        UserEntity capturedEntity = null;

        _mockUserStore.Setup(x => x.Create(It.IsAny<UserEntity>()))
            .ReturnsAsync(Guid.NewGuid())
            .Callback<UserEntity>(entity => capturedEntity = entity);

        // Act
        await _handler.Handle(command, CancellationToken.None);


        // Assert
        Assert.NotNull(capturedEntity);
        Assert.NotEqual("plainpassword", capturedEntity.PasswordHash);
        Assert.True(Crypt.Verify("plainpassword", capturedEntity.PasswordHash)); // Corrected: Verify plainpassword, not login
    }
}
