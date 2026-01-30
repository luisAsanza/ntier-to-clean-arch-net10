using AutoMapper;
using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Domain.Enums;
using CleanCRUDSolution.Web.Models.PersonModels.Data;

namespace CleanCRUDSolution.Web.Mapping
{
    /// <summary>
    /// AutoMapper profile to map between application DTOs and web view models for Person entities.
    /// </summary>
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonResponse, PersonDataVM>()
                .ConstructUsing((source, context) =>
                {
                    return new PersonDataVM
                    {
                        PersonName = source.Name,
                        Email = source.Email,
                        DateOfBirth = source.DateOfBirth,
                        Gender = source.Gender,
                        CountryId = source.CountryId,
                        CountryName = source.CountryName,
                        Address = source.Address,
                        ReceiveNewsLetters = source.ReceiveNewsLetters
                    };
                }
                );

            CreateMap<PersonResponse, UpdatePersonDataVM>()
            .ConstructUsing((source, context) =>
            {
                return new UpdatePersonDataVM
                {
                    Id = source.Id,
                    PersonName = source.Name,
                    Email = source.Email,
                    DateOfBirth = source.DateOfBirth,
                    Gender = source.Gender,
                    CountryId = source.CountryId,
                    CountryName = source.CountryName,
                    Address = source.Address,
                    ReceiveNewsLetters = source.ReceiveNewsLetters
                };
            }
            );

            CreateMap<PersonDataVM, PersonAddRequest>()
                .ConstructUsing((source, context) =>
                {
                    _ = Enum.TryParse<GenderOptions>(source.Gender, out var genderParsed);

                    return new PersonAddRequest(
                        source.PersonName ?? string.Empty,
                        source.Email,
                        source.DateOfBirth,
                        genderParsed,
                        source.CountryId,
                        source.Address,
                        source.ReceiveNewsLetters,
                        null
                        );
                }
                );

            CreateMap<UpdatePersonDataVM, PersonUpdateRequest>()
                .ConstructUsing((source, context) =>
                {
                    GenderOptions? genderParsed = null;
                    if (Enum.TryParse<GenderOptions>(source.Gender, out GenderOptions genderTryParse))
                        genderParsed = genderTryParse;

                    return new PersonUpdateRequest(
                        source.Id ?? Guid.Empty,
                        source.Email,
                        source.DateOfBirth,
                        genderParsed,
                        source.CountryId,
                        source.Address,
                        source.ReceiveNewsLetters,
                        null
                        );
                }
                );
        }
    }
}
