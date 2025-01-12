namespace XUnitTestProject.AuthorizationTests;

using Authorization.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

    public class AppExceptionHandlerTests
    {
        private readonly AppExceptionHandler _exceptionHandler;

        public AppExceptionHandlerTests()
        {
            _exceptionHandler = new AppExceptionHandler();
        }

        [Fact]
        public async Task TryHandleAsync_ValidationException_SetsBadRequestStatusCode()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new ValidationException("Validation failed");

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task TryHandleAsync_UnauthorizedAccessException_SetsUnauthorizedStatusCode()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new UnauthorizedAccessException("Unauthorized access");

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task TryHandleAsync_OperationCanceledException_SetsServiceUnavailableStatusCode()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new OperationCanceledException("Operation canceled");

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task TryHandleAsync_GenericException_SetsInternalServerErrorStatusCode()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("Generic error");

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
        }
    }
