using People.ServiceContracts.DTOs;

namespace People.ServiceContracts.Interfaces
{
    //service interface
    public interface IPersonsService
    {
        /// <summary>
        /// method to add the country
        /// </summary>
        /// <param name="request">request containing added country name</param>
        /// <returns>created or modified country info</returns>
        PersonResponse AddPerson(AddPersonRequest request);
        List<PersonResponse> GetPersonList();
        PersonResponse? GetPersonById(Guid? id);
        List<PersonResponse> GetFilteredPersons(string searchBy, string searchString );
        List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, bool ascending);
        PersonResponse UpdatePerson(UpdatePersonRequest? request);
        bool DeletePerson(Guid? personId);
    }
}
