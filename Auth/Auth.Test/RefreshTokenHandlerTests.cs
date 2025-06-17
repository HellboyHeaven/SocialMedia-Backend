using Moq;
using Core;
using Microsoft.EntityFrameworkCore.Storage;
using ServiceExeption.Exceptions;

namespace Application.Tests;

public class RefreshTokenHandlerTests
{
    private readonly Mock<IRefreshTokenStore> _mockRefreshTokenStore;
    private readonly Mock<IUserStore> _mockUserStore;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITokenProvider> _mockTokenProvider;
    private readonly Mock<IDbContextTransaction> _mockTransaction;
    private readonly RefreshTokenHandler _handler;

    public RefreshTokenHandlerTests()
    {
        _mockRefreshTokenStore = new Mock<IRefreshTokenStore>();
        _mockUserStore = new Mock<IUserStore>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTokenProvider = new Mock<ITokenProvider>();
        _mockTransaction = new Mock<IDbContextTransaction>();

        _mockUnitOfWork.Setup(x => x.BeginTrasnactionAsync())
            .ReturnsAsync(_mockTransaction.Object);

        _handler = new RefreshTokenHandler(
            _mockRefreshTokenStore.Object,
            _mockUserStore.Object,
            _mockUnitOfWork.Object,
            _mockTokenProvider.Object);
    }

    [Fact]
    public async Task Handle_ValidRefreshToken_ReturnsNewTokenModel()
    {
        // Arrange
        var command = new RefreshTokenCommand("valid_refresh_token");
        var userId = Guid.NewGuid();
        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = userId,
            Token = "valid_refresh_token",
            UserAgent = "Chrome",
            ExpiredAt = DateTime.UtcNow.AddDays(1)
        };
        var userEntity = new UserEntity
        {
            Id = userId,
            Login = "testuser",
            Role = UserRole.User,
            PasswordHash = ""
        };

        _mockRefreshTokenStore.Setup(x => x.GetByToken("valid_refresh_token"))
            .ReturnsAsync(refreshTokenEntity);
        _mockUserStore.Setup(x => x.GetById(userId))
            .ReturnsAsync(userEntity);
        _mockTokenProvider.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserRole>()))
            .Returns("new_access_token");
        _mockTokenProvider.Setup(x => x.GenerateRefreshToken())
            .Returns("new_refresh_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("new_access_token", result.AccessToken);
        Assert.Equal("new_refresh_token", result.RefreshToken);

        _mockRefreshTokenStore.Verify(x => x.Update(It.IsAny<RefreshTokenEntity>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        _mockTransaction.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once); // FIX APPLIED HERE
    }

    [Fact]
    public async Task Handle_TokenNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new RefreshTokenCommand("invalid_token");

        _mockRefreshTokenStore.Setup(x => x.GetByToken("invalid_token"))
            .ReturnsAsync((RefreshTokenEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _mockTransaction.Verify(x => x.RollbackAsync(CancellationToken.None), Times.Once); // FIX APPLIED HERE
    }

    [Fact]
    public async Task Handle_ExpiredToken_ThrowsUnauthorizedException()
    {
        // Arrange
        var command = new RefreshTokenCommand("expired_token");
        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = Guid.NewGuid(),
            Token = "expired_token",
            UserAgent = "Chrome",
            ExpiredAt = DateTime.UtcNow.AddDays(-1) // Expired
        };

        _mockRefreshTokenStore.Setup(x => x.GetByToken("expired_token"))
            .ReturnsAsync(refreshTokenEntity);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _mockTransaction.Verify(x => x.RollbackAsync(CancellationToken.None), Times.Once); // FIX APPLIED HERE
    }
}
