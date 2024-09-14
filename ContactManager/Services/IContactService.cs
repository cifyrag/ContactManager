using System.Linq.Expressions;
using ContactManager.Models;

namespace ContactManager.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> Get();
        Task<Contact> GetContact(Expression<Func<Contact,bool>> filter);
        Task<bool> EditContact(Contact contact);
        Task<bool> DeleteContact(Expression<Func<Contact,bool>> filter);
        Task<bool> CreateContact(Contact contact);
        Task<bool> ContactExists(Expression<Func<Contact,bool>> filter);
    }
}
