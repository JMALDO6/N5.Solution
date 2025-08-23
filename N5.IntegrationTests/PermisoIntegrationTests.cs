using FluentAssertions;
using IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using N5.Application.DTOs.Permission;
using N5.Domain.Entities;
using N5.Domain.Interfaces;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests
{
    public class PermisoIntegrationTests : IClassFixture<ApiTestFixture>
    {
        private readonly HttpClient _client;
        private readonly IPermissionElasticService _elasticService;

        public PermisoIntegrationTests(ApiTestFixture fixture)
        {
            _client = fixture.CreateClient();
            var scope = fixture.Services.CreateScope();
            _elasticService = scope.ServiceProvider.GetRequiredService<IPermissionElasticService>();
        }

        [Fact]
        public async Task Should_Request_Permission_And_Return_Id()
        {
            var payload = new { employeeName = "fakeName_IT", employeeLastName = "FakeLastName_IT", permissionTypeId = 1, permissionDate = DateTime.UtcNow };

            var response = await _client.PostAsJsonAsync("/api/Permissions/request-permission", payload);
            response.EnsureSuccessStatusCode();

            var id = await response.Content.ReadAsStringAsync();
            id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_Index_And_Query_Permission_When_Valid_Id_Is_Used()
        {
            // Step 1: Create a permission
            var payload = new { employeeName = "fakeNameCreate_IT", employeeLastName = "FakeLastNameCreate_IT", permissionTypeId = 1, permissionDate = DateTime.UtcNow };

            var createResponse = await _client.PostAsJsonAsync("/api/Permissions/request-permission", payload);
            createResponse.EnsureSuccessStatusCode();

            var createdId = await createResponse.Content.ReadFromJsonAsync<int>();
            Assert.NotNull(createdId);

            // Step 2: Ensure the document is indexed in Elasticsearch
            PermissionDocument? indexed = null;
            int retries = 10;

            for (int i = 0; i < retries; i++)
            {
                indexed = await _elasticService.GetPermissionByIdAsync(createdId);

                if (indexed != null)
                    break;

                await Task.Delay(500); // wait half a second for kafka to process
            }

            // Step 3: Validate the indexed document
            Assert.NotNull(indexed);
            Assert.Equal("fakeNameCreate_IT", indexed.EmployeeForename);
            Assert.Equal("FakeLastNameCreate_IT", indexed.EmployeeSurname);
            Assert.Equal(1, indexed.PermissionTypeId);

            // Step 4: Query the permission by ID
            var queryResponse = await _client.GetAsync($"/api/Permissions/{createdId}");
            queryResponse.EnsureSuccessStatusCode();

            var permission = await queryResponse.Content.ReadFromJsonAsync<PermissionDto>();

            // Step 5: Validate the response
            Assert.Equal("fakeNameCreate_IT", permission?.EmployeeName);
            Assert.Equal("FakeLastNameCreate_IT", permission?.EmployeeLastName);
            Assert.Equal(1, permission?.PermissionTypeId);
        }

        [Fact]
        public async Task Should_Request_And_Update_Permission_And_Return_Id()
        {
            // Step 1: Request a permission
            var payload = new { employeeName = "fakeNameUpdate_IT", employeeLastName = "FakeLastNameUpdate_IT", permissionTypeId = 1, permissionDate = DateTime.UtcNow };
            var response = await _client.PostAsJsonAsync("/api/Permissions/request-permission", payload);
            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadAsStringAsync();
            id.Should().NotBeNullOrEmpty();

            // Step 2: Update the permission
            var updatePayload = new { id, employeeName = "updatedName_IT", employeeLastName = "UpdatedLastName_IT", permissionTypeId = 2, permissionDate = DateTime.UtcNow.AddDays(1) };
            var updateResponse = await _client.PatchAsJsonAsync("/api/Permissions/modify-permission", updatePayload);
            updateResponse.EnsureSuccessStatusCode();
            var updatedId = await updateResponse.Content.ReadAsStringAsync();
            updatedId.Should().NotBeNullOrEmpty();
        }
    }
}