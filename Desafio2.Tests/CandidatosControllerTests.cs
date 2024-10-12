using BO200360_PD200491_Desafio2.Controllers;
using BO200360_PD200491_Desafio2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Desafio2.Tests
{
    public class CandidatosControllerTests
    {
        [Fact]
        public async Task PostCandidato_ReturnsCreatedAtActionResult_WhenCandidatoIsValid()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();

            // Crear un UserManager<IdentityUser> falso usando Moq
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );

            // Crear una configuración simulada
            var configMock = new Mock<IConfiguration>();

            var controller = new CandidatosController(context, userManagerMock.Object, configMock.Object);
            var candidato = new Candidato
            {
                Nombre = "John",
                Apellidos = "Doe",
                Telefono = "12345678",
                Email = "john.doe@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Contrasena = "password123"
            };

            // Act
            var result = await controller.PostCandidato(candidato);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCandidato = Assert.IsType<Candidato>(actionResult.Value);
            Assert.Equal(candidato.Nombre, returnedCandidato.Nombre);
        }

        [Fact]
        public async Task PostCandidato_GeneratesCorrectCodigoFormat()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();

            // Crear un UserManager<IdentityUser> falso usando Moq
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );

            // Crear una configuración simulada
            var configMock = new Mock<IConfiguration>();

            var controller = new CandidatosController(context, userManagerMock.Object, configMock.Object);

            var candidato = new Candidato
            {
                Nombre = "John",
                Apellidos = "Doe Peters",
                Telefono = "12345678",
                Email = "john.doe@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Contrasena = "password123"
            };

            // Act
            var result = await controller.PostCandidato(candidato);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCandidato = Assert.IsType<Candidato>(actionResult.Value);

            // Verificar el formato del código: 2 letras y 8 números
            var codigo = returnedCandidato.Codigo;
            var regex = new Regex(@"^[A-Z]{2}\d{8}$");

            Assert.Matches(regex, codigo);
        }

        [Fact]
        public async Task PostCandidato_HashesPasswordBeforeSaving()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();

            // Crear un UserManager<IdentityUser> falso usando Moq
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );

            // Crear una configuración simulada
            var configMock = new Mock<IConfiguration>();

            var controller = new CandidatosController(context, userManagerMock.Object, configMock.Object);

            var plainTextPassword = "password123";
            var candidato = new Candidato
            {
                Nombre = "John",
                Apellidos = "Doe Peters",
                Telefono = "12345678",
                Email = "john.doe@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Contrasena = plainTextPassword // Usamos una contraseña en texto plano
            };

            // Act
            var result = await controller.PostCandidato(candidato);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCandidato = Assert.IsType<Candidato>(actionResult.Value);

            // Verificamos que la contraseña no sea la misma que la ingresada (ya que debe estar hasheada)
            Assert.NotEqual(plainTextPassword, returnedCandidato.Contrasena);

            // Verificamos que la contraseña fue hasheada usando bcrypt
            Assert.True(BCrypt.Net.BCrypt.Verify(plainTextPassword, returnedCandidato.Contrasena));
        }
    }
}
