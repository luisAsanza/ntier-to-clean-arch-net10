# Unit Test Guidelines � CleanCRUDSolution

Purpose
- Document organization, naming, test priorities and frameworks used for the `CleanCRUDSolution.UnitTests` project.
- Provide a single-reference file to keep tests consistent and discoverable.

Suggested location
- This file is located in the UnitTests project so it travels with tests and is easy to find:
  - `tests/CleanCRUDSolution.UnitTests/Docs/UNIT_TESTS_GUIDELINES.md`

Top-level principles
- Test behavior, not implementation.
- Keep unit tests fast and deterministic; mock external dependencies (repositories, I/O, network).
- This project is for unit tests only; integration tests should go in `CleanCRUDSolution.IntegrationTests` with real infrastructure.

Folder structure (under `tests/CleanCRUDSolution.UnitTests`)
- `Domain/Entities` � Entity and value-object tests (e.g., `CountryTests.cs`)
- `Application/Features/<FeatureName>` � Services, use-cases, handlers, validators, and mappers (e.g., `CountriesServiceTests.cs`)
- `Presentation/Pages/<AreaOrFeature>` � Razor PageModel logic (unit-test PageModel methods and ModelState behavior)
- `Infrastructure` � Only small adapter logic; Mock infrastructure dependencies in application layer tests, but if an adapter has meaningful logic, it can be tested here.
- `Common` � Test fixtures, shared builders, test doubles, and helper utilities
- `Docs` � This guidelines file and any test-related documentation

File & test naming
- Test class files: `<ClassUnderTest>Tests.cs` (e.g., `CountriesServiceTests.cs`)
- Test methods: `MethodUnderTest_StateUnderTest_ExpectedBehavior`
- Keep one logical SUT per test class where possible.

Test priorities
1. Domain invariants and business rules (entities, domain services)
2. Application layer (use-cases, services, command/query handlers)
3. Presentation PageModels (page handlers and routing decisions)
4. Infrastructure adapters (only when they contain meaningful logic)

Frameworks and tooling used in this project
- xUnit � test runner / unit-test framework
- FluentAssertions � expressive assertions
- NSubstitute � mocking framework
- AutoFixture � automatic test data generation

Recommended packages to add to `tests/CleanCRUDSolution.UnitTests` (example)
- `xunit`
- `xunit.runner.visualstudio`
- `FluentAssertions`
- `NSubstitute`
- `AutoFixture`

Quick example: test location for `CountriesService`
- `tests/CleanCRUDSolution.UnitTests/Application/Features/Countries/CountriesServiceTests.cs`

Running tests
- From repo root: `dotnet test tests\CleanCRUDSolution.UnitTests`

CI / PR guidance
- Run unit tests on every PR.
- Prefer small, focused test changes per PR.
- Add tests for bug fixes and any change to business logic.

Maintenance
- Keep `Docs/UNIT_TESTS_GUIDELINES.md` in sync with conventions.
- When adding a new framework or major pattern change, update this file.

Contact
- If unclear where a test belongs, place it in `Application/Features/<FeatureName>` by default.
