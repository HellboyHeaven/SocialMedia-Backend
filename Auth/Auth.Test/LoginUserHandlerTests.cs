using Moq;

using Core;
using Application;
using ServiceExeption.Exceptions;

using Microsoft.EntityFrameworkCore.Storage;
using Crypt = BCrypt.Net.BCrypt;

namespace Application.Tests
{
    public class LoginUserHandlerTests
    {
        private readonly Mock<IUserStore> _mockUserStore;
        private readonly Mock<IRefreshTokenStore> _mockRefreshTokenStore;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ITokenProvider> _mockTokenProvider;
        private readonly Mock<IDbContextTransaction> _mockTransaction;
        private readonly LoginUserHandler _handler;

        public LoginUserHandlerTests()
        {
            _mockUserStore = new Mock<IUserStore>();
            _mockRefreshTokenStore = new Mock<IRefreshTokenStore>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTokenProvider = new Mock<ITokenProvider>();
            _mockTransaction = new Mock<IDbContextTransaction>();

            _mockUnitOfWork.Setup(x => x.BeginTrasnactionAsync())
                .ReturnsAsync(_mockTransaction.Object);

            _handler = new LoginUserHandler(
                _mockUserStore.Object,
                _mockRefreshTokenStore.Object,
                _mockUnitOfWork.Object,
                _mockTokenProvider.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsTokenModel()
        {
            // Arrange
            var command = new LoginUserCommand("testuser", "password", "Chrome");
            var hashedPassword = Crypt.HashPassword("password");
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = "testuser",
                PasswordHash = hashedPassword,
                Role = UserRole.User
            };

            _mockUserStore.Setup(x => x.GetByLogin("testuser"))
                .ReturnsAsync(userEntity);
            _mockTokenProvider.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserRole>()))
                .Returns("access_token");
            _mockTokenProvider.Setup(x => x.GenerateRefreshToken())
                .Returns("refresh_token");

            _mockRefreshTokenStore.Setup(x => x.Create(It.IsAny<RefreshTokenEntity>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act - Added missing Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("access_token", result.AccessToken);
            Assert.Equal("refresh_token", result.RefreshToken);

            _mockRefreshTokenStore.Verify(x => x.Create(It.IsAny<RefreshTokenEntity>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mockTransaction.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var command = new LoginUserCommand("testuser", "wrongpassword", "Chrome");
            var hashedPassword = Crypt.HashPassword("correctpassword");
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = "testuser",
                PasswordHash = hashedPassword,
                Role = UserRole.User // Changed to enum
            };

            _mockUserStore.Setup(x => x.GetByLogin("testuser"))
                .ReturnsAsync(userEntity);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _mockTransaction.Verify(x => x.RollbackAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsException()
        {
            // Arrange
            var command = new LoginUserCommand("nonexistent", "password", "Chrome");

            _mockUserStore.Setup(x => x.GetByLogin("nonexistent"))
                .ReturnsAsync((UserEntity)null); // Changed to return null for not found scenario

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => // Changed to NotFoundException
                _handler.Handle(command, CancellationToken.None));

            _mockTransaction.Verify(x => x.RollbackAsync(CancellationToken.None), Times.Once);
        }
    }


}
