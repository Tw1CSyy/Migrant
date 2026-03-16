using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrant.Application.Abstractions;
using Migrant.Application.Services;
using Migrant.Data.Abstractions;
using Moq;

namespace Migrant.Tests.Tests
{
    public class PassportUpdateServiceTests
    {
        /// <summary>
        /// Основной тест приложения, вызывает полный цикл передачи данных в базу
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAsync_Should_CallImporter_And_Merger()
        {
            var source = new Mock<IPassportSource>();
            var provider = new Mock<IServiceProvider>();
            var importer = new Mock<IPassportImportService>();
            var merger = new Mock<IPassportMergeService>();

            var config = new Mock<IConfiguration>();

            var stream = new MemoryStream();

            source.Setup(x => x.GetFileStreamAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(stream);

            var runner = new PassportUpdateRunner(
                source.Object,
                provider.Object,
                config.Object,
                merger.Object,
                importer.Object
            );

            await runner.RunAsync(CancellationToken.None);

            importer.Verify(x => x.ImportAsync(stream, It.IsAny<CancellationToken>()), Times.Once);
            merger.Verify(x => x.MergeAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Проверка порядка вызова Import - Merge
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAsync_ShouldCallImportBeforeMerge()
        {
            var sequence = new MockSequence();

            var source = new Mock<IPassportSource>();
            var importer = new Mock<IPassportImportService>();
            var merger = new Mock<IPassportMergeService>();
            var config = new Mock<IConfiguration>();

            var stream = new MemoryStream();
            source.Setup(x => x.GetFileStreamAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(stream);

            importer.InSequence(sequence)
                .Setup(x => x.ImportAsync(stream, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            merger.InSequence(sequence)
                .Setup(x => x.MergeAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var runner = new PassportUpdateRunner(
                source.Object, null, config.Object, merger.Object, importer.Object);

            await runner.RunAsync(CancellationToken.None);
        }

        /// <summary>
        /// Если ImportAsync падает — MergeAsync не вызывается
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAsync_WhenImportFails_ShouldNotCallMerge()
        {
            var source = new Mock<IPassportSource>();
            var importer = new Mock<IPassportImportService>();
            var merger = new Mock<IPassportMergeService>();
            var config = new Mock<IConfiguration>();

            var stream = new MemoryStream();

            source.Setup(x => x.GetFileStreamAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(stream);

            importer.Setup(x => x.ImportAsync(stream, It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception());

            var runner = new PassportUpdateRunner(
                source.Object, null, config.Object, merger.Object, importer.Object);

            await Assert.ThrowsAsync<Exception>(() => runner.RunAsync(CancellationToken.None));

            merger.Verify(x => x.MergeAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
