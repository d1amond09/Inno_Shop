using FluentAssertions;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Inno_Shop.Services.ProductAPI;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace Inno_Shop.Services.IntegrationTests;

public class ProductApiTests : IClassFixture<CustomWebApplicationFactory<ProductAPI.Program>>
{
    private readonly HttpClient _client;

    public ProductApiTests(CustomWebApplicationFactory<ProductAPI.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOk()
    {
        var loginContent = new StringContent(JsonConvert.SerializeObject(new { Username = "your-username", Password = "your-password" }), Encoding.UTF8, "application/json");
        var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);

        loginResponse.EnsureSuccessStatusCode(); // Проверка, успешен ли ответ
        var responseString = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<TokenDto>(responseString).AccessToken;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/products");
        response.EnsureSuccessStatusCode(); // Может вызвать исключение 404, если не найдено
    }

    // Ваши тесты здесь
    [Fact]
    public async Task GetProducts_ReturnsOk_WhenCalled()
    {
        // Arrange
        var expectedCount = 5; // Предполагая, что вы ожидаете 5 продуктов

        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().HaveCount(expectedCount);
    }

    private async Task<string> RegisterAndGetJwtToken(string email, string password)
    {
        // Регистрация пользователя
        var registerData = new UserForRegistrationDto
        {
            UserName = email,
            Email = email,
            Password = password
        };

        var registerResponse = await _client.PostAsJsonAsync("/api/authentication/signup", registerData); // Замените путь на правильный
        registerResponse.EnsureSuccessStatusCode();

        // Логин пользователя
        var loginData = new UserForAuthenticationDto
        {
            UserName = email,
            Password = password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/authentication/login", loginData);
        loginResponse.EnsureSuccessStatusCode();

        var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<TokenDto>(); // Предположим, что вы возвращаете токен в ответе
        return tokenResponse?.AccessToken; // Возвращаем JWT токен
    }

    [Fact]
    public async Task GetProductsForUser_ShouldReturnProducts_WhenUserIsAuthenticated()
    {
        // Arrange
        var token = await RegisterAndGetJwtToken("test@example.com", "password123");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/products?userId=some-user-id");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateProduct_ShouldCreateProduct_WhenUserIsAuthenticated()
    {
        // Arrange
        var token = await RegisterAndGetJwtToken("test@example.com", "password123");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var newProduct = new ProductForCreationDto { Name = "New Product" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", newProduct);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        createdProduct.Should().NotBeNull();
        createdProduct.Name.Should().Be(newProduct.Name);
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProduct_WhenUserIsAuthenticated()
    {
        // Arrange
        var token = await RegisterAndGetJwtToken("test@example.com", "password123");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var existingProductId = Guid.NewGuid(); // Укажите нужный ID продукта
        var updatedProduct = new ProductForUpdateDto { Name = "Updated Product" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{existingProductId}", updatedProduct);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct_WhenUserIsAuthenticated()
    {
        // Arrange
        var token = await RegisterAndGetJwtToken("test@example.com", "password123");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var productIdToDelete = Guid.NewGuid(); // Укажите нужный ID продукта для удаления

        // Act
        var response = await _client.DeleteAsync($"/api/products/{productIdToDelete}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}