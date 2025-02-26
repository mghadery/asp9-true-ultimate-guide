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
        Task<PersonResponse> AddPerson(AddPersonRequest request);
        Task<List<PersonResponse>> GetPersonList();
        Task<PersonResponse?> GetPersonById(Guid? id);
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string searchString );
        List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, bool ascending);
        Task<PersonResponse> UpdatePerson(UpdatePersonRequest? request);
        Task<bool> DeletePerson(Guid? personId);
        Task<MemoryStream> GetExcel();
        Task<MemoryStream> GetCsv();
    }
}
