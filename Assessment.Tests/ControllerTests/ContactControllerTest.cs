using Assessment.Controllers;
using Assessment.Interfaces;
using Assessment.Models.Requests;
using Assessment.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Assessment.Tests.ControllerTests
{
    public class ContactControllerTest
    {
        public Mock<IContactRepository> _contactRepository;

        public Mock<ILogger<ContactController>> _logger;


        public ContactController _controller;

        public ContactControllerTest()
        {
            _contactRepository = new Mock<IContactRepository>();
            _logger = new Mock<ILogger<ContactController>>();
            _controller = new ContactController(_contactRepository.Object, _logger.Object);
        }


        [Fact]
        public async Task GetContacts_RepositoryReturnsResult_ReturnsSuccess()
        {
            //Arrange
            var contacts = new List<ContactResponse>
            {
                 new ContactResponse
                 {
                      Id = Guid.NewGuid(),
                       Address = "Abby",
                       Name = "Test Name"
                 }
            };
            _contactRepository.Setup(x => x.GetContacts()).ReturnsAsync(contacts);


            //Act 
            var response = await _controller.GetContacts();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);
            _contactRepository.Verify(x => x.GetContacts(), Times.Once);
        }

        [Fact]
        public async Task GetContacts_RepositoryThrowsException_ReturnsNull()
        {
            //Arrange
            var contacts = new List<ContactResponse>
            {
                 new ContactResponse
                 {
                      Id = Guid.NewGuid(),
                       Address = "Abby",
                       Name = "Test Name"
                 }
            };
            _contactRepository.Setup(x => x.GetContacts()).Throws(new Exception());

            //Act Assert 

            IActionResult response = null;
            try
            {
                response = await _controller.GetContacts();
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Null(response);
                _contactRepository.Verify(x => x.GetContacts(), Times.Once);
            }
        }


        [Fact]
        public async Task SaveContact_RepositoryReturnsResult_ContactDoestNotExist_ReturnsSuccess()
        {
            //Arrange 
            _contactRepository.Setup(x => x.AddContact(It.IsAny<ContactRequest>()));

            _contactRepository.Setup(x => x.Exists(It.IsAny<string>())).ReturnsAsync(false);

            //Act 
            var response = await _controller.SaveContact(new ContactRequest
            {
                Name = "test name",
                Address = "Test Address"
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);
            _contactRepository.Verify(x => x.AddContact(It.IsAny<ContactRequest>()), Times.Once);
            _contactRepository.Verify(x => x.Exists(It.IsAny<string>()),Times.Once);
        }

        [Fact]
        public async Task SaveContact_RepositoryReturnsResult_ContactExist_ReturnsSuccess()
        {
            //Arrange 
            _contactRepository.Setup(x => x.AddContact(It.IsAny<ContactRequest>()));

            _contactRepository.Setup(x => x.Exists(It.IsAny<string>())).ReturnsAsync(true);

            //Act 
            var response = await _controller.SaveContact(new ContactRequest
            {
                Name = "test name",
                Address = "Test Address"
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
            _contactRepository.Verify(x => x.AddContact(It.IsAny<ContactRequest>()), Times.Never);
            _contactRepository.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SaveContact_RepositoryThrowsException_ReturnsNull()
        {
            //Arrange
            _contactRepository.Setup(x => x.AddContact(It.IsAny<ContactRequest>())).Throws(new Exception());

            //Act 
            IActionResult response = null;
            try
            {
                response = await _controller.SaveContact(new ContactRequest
                {
                    Name = "test name",
                    Address = "Test Address"
                });
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Null(response);
                _contactRepository.Verify(x => x.AddContact(It.IsAny<ContactRequest>()), Times.Once);
            }
        }

        [Fact]
        public async Task UpdateContact_RepositoryReturnsResult_ReturnsSuccess()
        {
            //Arrange 
            _contactRepository.Setup(x => x.UpdateContact(It.IsAny<ContactUpdate>(), It.IsAny<Guid>()));

            //Act 
            var response = await _controller.UpdateContact(Guid.NewGuid(), new ContactUpdate
            {
                Name = "test name",
                Address = "Test Address"
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);
            _contactRepository.Verify(x => x.UpdateContact(It.IsAny<ContactUpdate>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateContact_RepositoryThrowsException_ReturnsNull()
        {
            //Arrange
            _contactRepository.Setup(x => x.UpdateContact(It.IsAny<ContactUpdate>(), It.IsAny<Guid>())).Throws(new Exception());

            //Act 
            IActionResult response = null;
            try
            {
                response = await _controller.UpdateContact(Guid.NewGuid(), new ContactUpdate
                {
                    Name = "test name",
                    Address = "Test Address"
                });
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Null(response);
                _contactRepository.Verify(x => x.UpdateContact(It.IsAny<ContactUpdate>(), It.IsAny<Guid>()), Times.Once);
            }
        }

        [Fact]
        public async Task DeleteContact_RepositoryReturnsResult_ReturnsSuccess()
        {
            //Arrange 
            _contactRepository.Setup(x => x.DeleteContact(It.IsAny<Guid>()));

            //Act 
            var response = await _controller.DeleteContact(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);
            _contactRepository.Verify(x => x.DeleteContact(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteContact_RepositoryThrowsException_ReturnsNull()
        {
            //Arrange
            _contactRepository.Setup(x => x.DeleteContact(It.IsAny<Guid>())).Throws(new Exception());

            //Act 
            IActionResult response = null;
            try
            {
                response = await _controller.DeleteContact(Guid.NewGuid());
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Null(response);
                _contactRepository.Verify(x => x.DeleteContact(It.IsAny<Guid>()), Times.Once);
            }
        }
    }
}
