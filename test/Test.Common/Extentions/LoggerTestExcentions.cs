namespace Test.Infrastructure.Extentions;

internal static class LoggerTestExcentions
{
    public static Mock<ILogger<T>> VerifyErrorWasCalled<T>(this Mock<ILogger<T>> logger, string expectedMessage,
        Times? times = null)
    {
        return logger.VerifyLogging(expectedMessage, expectedLogLevel: LogLevel.Error, times: times);
    }

    public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage,
        LogLevel expectedLogLevel = LogLevel.Debug, Times? times = null)
    {
        times ??= Times.Once();

        Func<object, Type, bool> state = (v, t) =>
        {
            return v.ToString().Contains(expectedMessage);
        };

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), (Times)times);

        return logger;
    }
}