using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using XamarinApp.Model;

namespace XamarinApp.Helper
{
    public class FirebaseHelper
    {
        private readonly string ChildName = "Persons";

        readonly FirebaseClient firebase = new FirebaseClient("https://xamarin-app-68883.firebaseio.com/");

        public async Task<List<Person>> GetAllPersons()
        {
            return (await firebase
                .Child(ChildName)
                .OnceAsync<Person>()).Select(item => new Person
            {
                Name = item.Object.Name,
                PersonId = item.Object.PersonId,
                Phone = item.Object.Phone
            }).ToList();
        }

        public async Task AddPerson(string name, string phone)
        {
            await firebase
                .Child(ChildName)
                .PostAsync(new Person() { PersonId = Guid.NewGuid(), Name = name, Phone = phone });
        }

        public async Task<Person> GetPerson(Guid personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
                .Child(ChildName)
                .OnceAsync<Person>();
            return allPersons.FirstOrDefault(a => a.PersonId == personId);
        }

        public async Task<Person> GetPerson(string name)
        {
            var allPersons = await GetAllPersons();
            await firebase
                .Child(ChildName)
                .OnceAsync<Person>();
            return allPersons.FirstOrDefault(a => a.Name == name);
        }

        public async Task UpdatePerson(Guid personId, string name, string phone)
        {
            var toUpdatePerson = (await firebase
                .Child(ChildName)
                .OnceAsync<Person>()).FirstOrDefault(a => a.Object.PersonId == personId);

            await firebase
                .Child(ChildName)
                .Child(toUpdatePerson.Key)
                .PutAsync(new Person() { PersonId = personId, Name = name, Phone = phone });
        }

        public async Task DeletePerson(Guid personId)
        {
            var toDeletePerson = (await firebase
                .Child(ChildName)
                .OnceAsync<Person>()).FirstOrDefault(a => a.Object.PersonId == personId);
            await firebase.Child(ChildName).Child(toDeletePerson.Key).DeleteAsync();
        }
    }
}
