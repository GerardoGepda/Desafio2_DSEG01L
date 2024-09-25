using BO200360_PD200491_Desafio2.Controllers;
using BO200360_PD200491_Desafio2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio2.Tests
{
    public class CvsControllerTests
    {
        [Fact]
        public async Task PostCv_Should_Save_Cv()
        {
            // Arrange: Crear el contexto de base de datos en memoria
            var context = Setup.GetInMemoryDatabaseContext();

            // Crear un candidato para agregar a la base de datos
            var candidato = new Candidato
            {
                Codigo = "PD28374582",
                Nombre = "Pedro",
                Apellidos = "Pastor Diaz",
                Telefono = "12345678",
                Email = "pedro.diaz@example.com",
                FechaNacimiento = new System.DateTime(1990, 1, 1),
                Contrasena = "PasswordSegura123"
            };

            // Agregar el candidato al contexto
            context.Candidatos.Add(candidato);
            await context.SaveChangesAsync();

            // Crear el controlador de Cvs
            var controller = new CvsController(context);

            // Crear un objeto CV relacionado con el candidato
            var cv = new Cv
            {
                FormacionAcademica = "Ingeniería en Sistemas",
                ExperienciaProfesional = "5 años en desarrollo de software",
                ReferenciasPersonales = "Juan Pérez",
                Idiomas = "Español, Inglés",
                CamposActualizados = "Certificado en .NET",
                FechaCreacion = System.DateTime.Now,
                Estado = true,
                CandidatoId = candidato.Id // Asignar el ID del candidato existente
            };

            // Act: Llamar al método PostCv
            var result = await controller.PostCv(cv);

            // Assert: Verificar que el resultado sea una respuesta "Created"
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var savedCv = Assert.IsType<Cv>(createdAtActionResult.Value);
            Assert.Equal(cv.FormacionAcademica, savedCv.FormacionAcademica);
            Assert.Equal(candidato.Id, savedCv.CandidatoId);
        }

        [Fact]
        public async Task PostCv_Should_Return_BadRequest()
        {
            // Arrange: Crear el contexto de base de datos en memoria
            var context = Setup.GetInMemoryDatabaseContext();

            // Crear el controlador de Cvs
            var controller = new CvsController(context);

            // Crear un objeto CV con un CandidatoId inexistente
            var cv = new Cv
            {
                FormacionAcademica = "Ingeniería en Sistemas",
                ExperienciaProfesional = "5 años en desarrollo de software",
                ReferenciasPersonales = "Juan Pérez",
                Idiomas = "Español, Inglés",
                CamposActualizados = "Certificado en .NET",
                FechaCreacion = System.DateTime.Now,
                Estado = true,
                CandidatoId = 999 // CandidatoId que no existe en la base de datos
            };

            // Act: Llamar al método PostCv
            var result = await controller.PostCv(cv);

            // Assert: Verificar que el resultado sea una respuesta de error BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
