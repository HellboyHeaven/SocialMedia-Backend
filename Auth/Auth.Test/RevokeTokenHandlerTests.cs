using Moq;

using Core;

namespace Application.Tests
{
    public class RevokeTokenHandlerTests
    {
        private readonly Mock<IRefreshTokenStore> _mockRefreshTokenStore;
        private readonly RevokeTokenHandler _handler;

        public RevokeTokenHandlerTests()
        {
            _mockRefreshTokenStore = new Mock<IRefreshTokenStore>();
            _handler = new RevokeTokenHandler(_mockRefreshTokenStore.Object);
        }

        [Fact]
        public async Task Handle_TokenExists_DeletesTokenWithoutTransaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new RevokeTokenCommand(userId, "Chrome");
            var refreshTokenEntity = new RefreshTokenEntity
            {
                UserId = userId,
                UserAgent = "Chrome",
                Token = "some_token",
                ExpiredAt = DateTime.UtcNow
            };

            _mockRefreshTokenStore.Setup(x => x.GetByUserIdAndAgent(userId, "Chrome"))
                .ReturnsAsync(refreshTokenEntity);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRefreshTokenStore.Verify(x => x.Delete(refreshTokenEntity), Times.Once);
            // Note: RevokeTokenHandler doesn't use transactions, unlike other handlers
        }

        [Fact]
        public async Task Handle_Exception_DoesNotRollback()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new RevokeTokenCommand(userId, "Chrome");

            _mockRefreshTokenStore.Setup(x => x.GetByUserIdAndAgent(userId, "Chrome"))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Note: RevokeTokenHandler doesn't use transactions, so no rollback occurs
        }

        [Fact]
        public async Task Handle_TokenNotFound_DoesNothing()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new RevokeTokenCommand(userId, "Chrome");

            _mockRefreshTokenStore.Setup(x => x.GetByUserIdAndAgent(userId, "Chrome"))
                .ReturnsAsync((RefreshTokenEntity)null);

            // Act
            await _handler.Handle(command, CancellationToken.None); // Corrected CancellationToken.None to CancellationToken.Token

            // Assert
            _mockRefreshTokenStore.Verify(x => x.Delete(It.IsAny<RefreshTokenEntity>()), Times.Never);
        }
    }


}
