using ContactManager.Models;

namespace ContactManager.Services;

public static class CSVWorker
{
    public static async Task<IEnumerable<Contact>> ReadCSV(IFormFile csvFile)
    {
        using (var reader = new StreamReader(csvFile.OpenReadStream()))
        {
            var csvContent = await reader.ReadToEndAsync();
            var contacts = ParseCsvAsync(csvContent);
            
            if(contacts == null || contacts.Count == 0)
            {
                return new List<Contact>();
            }

            return contacts;
        }
    }
    
    private static List<Contact> ParseCsvAsync(string csvContent)
    {
        var contacts = new List<Contact>();
        var lines = csvContent.Split('\n').ToList();
        var fColumns = lines[0].Split(',');
        string[] expectedColumns = { "Name", "DateOfBirth", "Married", "Phone", "Salary\r" };
        bool isColumnNames = fColumns.SequenceEqual(expectedColumns);

        if (isColumnNames)
        {
            lines.RemoveAt(0);
        }

        foreach (var line in lines)
        {
            var columns = line.Split(',');

            if (columns.Length >= 5)
            {
                if (!DateTime.TryParse(columns[1], out DateTime dateOfBirth) || 
                    !bool.TryParse(columns[2], out bool married) ||
                    !decimal.TryParse(columns[4], out decimal salary))
                {
                    continue;
                }
                
                var contact = new Contact
                {
                    Name = columns[0],
                    DateOfBirth = dateOfBirth,
                    Married = married,
                    Phone = columns[3],
                    Salary = salary,
                };
                
                contacts.Add(contact);
            }
        }

        return contacts;
    }
}