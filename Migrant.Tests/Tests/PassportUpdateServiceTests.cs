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
    }
}
