using System.Linq.Expressions;
using ContactManager.Models;
using ContactManager.Repository;

namespace ContactManager.Services
{
    public class ContactService: IContactService
    {
        private readonly IGenericRepository<Contact> _contactRepository;
        private readonly ILogger<ContactService> _logger;
        
        public ContactService(IGenericRepository<Contact> contactRepository, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Contact>> Get()
        {
            try
            {
                var contactResult = await _contactRepository.GetListAsync<Contact>(asNoTracking:true);

                if (!contactResult.Success || contactResult.Data == null || contactResult.Data.Count() == 0)
                {
                    return new List<Contact>();
                }
            
                return contactResult.Data; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting contacts");
                throw;
            }
        }

        public async Task<Contact> GetContact(Expression<Func<Contact,bool>> filter)
        {
            try
            {
                var contactResult = await _contactRepository.GetSingleAsync<Contact>(filter, asNoTracking: true);

                if (!contactResult.Success || contactResult.Data == null)
                {
                    return null;
                }
            
                return contactResult.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting contact");
                throw;
            }
        }

        public async Task<bool> ContactExists(Expression<Func<Contact,bool>> filter)
        {
            try
            {
                var exists = await _contactRepository.ExistsAsync(filter);
            
                return exists.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking contact exists");
                throw;
            }
        }

        public async Task<bool> EditContact(Contact contact)
        {
            try
            {
                var contactResult = await _contactRepository.GetSingleAsync<Contact>(x => x.Phone == contact.Phone, asNoTracking: true);
            
                if (!contactResult.Success || contactResult.Data == null)
                {
                    return false;
                }
            
                var updateResult = await _contactRepository.UpdateAsync(contact);
            
                return updateResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating contact");
                throw;
            }
        }

        public async Task<bool> DeleteContact(Expression<Func<Contact,bool>> filter)
        {
            try
            {
                var contactResult = await _contactRepository.GetSingleAsync<Contact>(filter, asNoTracking: true);
            
                if (!contactResult.Success || contactResult.Data == null)
                {
                    return false;
                }
            
                var removeResult = await _contactRepository.RemoveAsync(contactResult.Data);
            
                return removeResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting contact");
                throw;
            }
        }

        public async Task<bool> CreateContact(Contact contact)
        {
            try
            {
                var contactResult = await _contactRepository.GetSingleAsync<Contact>(x => x.Phone == contact.Phone, asNoTracking: true);
            
                if (!contactResult.Success || contactResult.Data != null)
                {
                    return false;
                }

                var createResult = await _contactRepository.AddAsync(contact);

                return createResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating contact");
                throw;
            }
        }
    }
}
