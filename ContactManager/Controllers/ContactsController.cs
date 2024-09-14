using ContactManager.Models;
using ContactManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var contacts = await _contactService.Get();
            
                return View(contacts);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while getting contacts";
                
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> GetContact(string phone)
        {
            try
            {
                var contact = await _contactService.GetContact(x => x.Phone == phone);

                if (contact == null)
                {
                    TempData["Error"] = "Contact is not found";
                    return RedirectToAction(nameof(Index));
                }

                return View(contact);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while getting contact";
                
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> EditContact(string phone)
        {
            try
            {
                var exist = await _contactService.ContactExists(x => x.Phone == phone);
            
                if (!exist)
                {
                    TempData["Error"] = "Contact cannot be null";
                
                    return RedirectToAction(nameof(Index));
                }
                var contact = await _contactService.GetContact(x => x.Phone == phone);
            
                if (contact == null)
                {
                    TempData["Error"] = "Contact is not found";
               
                    return RedirectToAction(nameof(Index));
                }

                return View(contact);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while editing contact";
                
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> EditContactPut(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    TempData["Error"] = "Contact cannot be null";
                
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    TempData["Error"] = await GetErrorsFromModelState();
               
                    return RedirectToAction(nameof(Index));
                }

                var result = await _contactService.EditContact(contact);

                if (result)
                {
                    TempData["Success"] = "Successfully updated";
                
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Something went wrong";
            
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while updating contact";
                
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> DeleteContact(string phone)
        {
            try
            {
                var exist = await _contactService.ContactExists(x => x.Phone == phone);
            
                if (!exist)
                {
                    TempData["Error"] = "Contact cannot be null";
                
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    TempData["Error"] = await GetErrorsFromModelState();
                
                    return RedirectToAction(nameof(Index));
                }
                bool result = await _contactService.DeleteContact(x => x.Phone == phone);

                if (result)
                {
                    TempData["Success"] = "Successfully deleted";
                
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Something went wrong";
            
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while deleting contact";
                
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> CreateContact()
        {
            return View();
        }

        public async Task<IActionResult> UploadAsync(IFormFile csvFile)
        {
            try
            {
                if (csvFile == null || csvFile.Length == 0)
                {
                    TempData["Error"] = "No file selected";
                
                    return RedirectToAction(nameof(CreateContact));
                }
                var fileExtension = Path.GetExtension(csvFile.FileName);
            
                if (fileExtension != ".csv")
                {
                    TempData["Error"] = "Please upload a valid CSV file.";
                
                    return RedirectToAction(nameof(CreateContact));
                }
            
                var contacts = await CSVWorker.ReadCSV(csvFile);
            
                foreach (var contact in contacts )
                {
                    if (!ModelState.IsValid || !TryValidateModel(contact))
                    {
                        TempData["Error"] = await GetErrorsFromModelState();

                        return RedirectToAction(nameof(CreateContact));
                    }
                
                    var contactExists = await _contactService.ContactExists(x => x.Phone == contact.Phone);
                
                    if (contactExists)
                    {
                        TempData["Error"] = "Contact already exists";
                        return RedirectToAction(nameof(CreateContact));
                    }
                    else
                    {
                        await _contactService.CreateContact(contact);
                        TempData["Success"] = "Successfully added";
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while uploading contacts";
                
                return RedirectToAction(nameof(CreateContact));
            }
        }
        
        private async Task<string> GetErrorsFromModelState()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return string.Join("/n", errors);
        }
    }
}
