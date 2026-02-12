# Integration Tests Guidelines

Purpose
- Describe how to add integration tests for the Web project using test host and HTTP client.

Location
- Put tests in `tests/CleanCRUDSolution.IntegrationTests` project.

Dependencies
- Use `Microsoft.AspNetCore.Mvc.Testing` and `xunit` (already referenced).
- Use `FluentAssertions` for assertions.

Naming
- Name test classes after the controller or feature, e.g. `PersonsControllerTests`.
- Use `Should` style for test names: `GetPersons_ReturnsOkAndList`.

Test setup
- Derive from or use `WebApplicationFactory<TProgram>` where `TProgram` is the `Program` type from the Web project.
- Configure the factory to override environment and services for isolation.

Seeding & isolation
- Seed only minimal data required for the test. Prefer in-memory stores or test-specific DB instances.
- Reset or recreate the test host between tests when stateful components are used.

Configuration overrides
- Use `WithWebHostBuilder` and `ConfigureAppConfiguration` or `ConfigureServices` to apply test settings (e.g., use in-memory DB, test cache, or stubbed services).

Common patterns
- Arrange: create `WebApplicationFactory`, configure, and get `HttpClient`.
- Act: call endpoints with `HttpClient`.
- Assert: verify status codes, response shape, and business behavior.

Form posts & antiforgery
- When testing Razor form POSTs, `GET` the edit/create page first to collect cookies and the antiforgery token (if the app uses one).
- Extract `__RequestVerificationToken` from the returned HTML and include it in the `FormUrlEncodedContent` for the subsequent `POST`.
- Use `new FormUrlEncodedContent(IEnumerable<KeyValuePair<string,string>>)` to post form fields that match the view model names (e.g. `PersonData.Id`, `PersonData.Email`, etc.).
- Date-only fields should be formatted as `yyyy-MM-dd` when posting (the `Edit` view expects this format).

Assertion patterns for web forms
- Redirects: assert `postResponse.StatusCode` is `Redirect` and `postResponse.Headers.Location` matches the expected path (e.g. `/`).
- Validation errors: when posting invalid data the endpoint will usually return `OK` with the page and validation messages; assert the returned HTML contains the expected validation strings.
- Antiforgery handling: test both the success case (include token) and the failure case (omit token). In some test environments a missing antiforgery token may produce `BadRequest`, `Forbidden`, or `NotFound` — assert against the acceptable set.

Practical tips from existing tests
- Create the `HttpClient` with `AllowAutoRedirect = false` and `HandleCookies = true` so you can assert redirects and the cookies set on `GET` are sent automatically on `POST`.
- When building form data from entities, include all view-bound fields (hidden fields, disabled fields that have corresponding hidden inputs, checkboxes as `true`/`false`, etc.).
- For radio buttons and selects, use the underlying value (e.g. numeric enum value for gender, or `Guid` string for `CountryId`).

Example skeleton

```csharp
public class PersonsControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PersonsControllerIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPersons_ReturnsOk()
    {
        var response = await _client.GetAsync("/persons");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

Running tests
- From repository root: `dotnet test tests/CleanCRUDSolution.IntegrationTests`.

Tips
- Keep integration tests focused on cross-cutting behavior, not unit-level logic.
- Use smaller, targeted integration tests rather than large end-to-end scenarios where possible.
- Keep test data stable and deterministic.

Questions
- If you want, I can add an example factory with in-memory DB and a sample test file.
