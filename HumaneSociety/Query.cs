﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();


            
            // if the address isn't fou nd in the Db, create and insert it
            if (updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;

            // submit changes
            db.SubmitChanges();
        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////

        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            
            switch (crudOperation)
            {
                case "update":
                    var updateEmployee = new Employee();
                    updateEmployee = db.Employees.Where(e => e.FirstName == updateEmployee.FirstName && e.LastName == updateEmployee.LastName && e.Email == updateEmployee.Email && e.UserName == updateEmployee.UserName && e.EmployeeNumber == updateEmployee.EmployeeNumber).FirstOrDefault();
                    db.Employees.InsertOnSubmit(updateEmployee);
                    db.SubmitChanges();
                    break;
                case "read":
                    var readEmployee = employee;
                   readEmployee = db.Employees.Where(e => e.FirstName == readEmployee.FirstName && e.LastName == readEmployee.LastName && e.Email == readEmployee.Email && e.UserName == readEmployee.UserName && e.EmployeeNumber == readEmployee.EmployeeNumber).FirstOrDefault();
                    Console.WriteLine(readEmployee);
                    break;
                case "delete":
                    var deletemployee = employee;
                    deletemployee = db.Employees.Where(e => e.FirstName == deletemployee.FirstName && e.LastName == deletemployee.LastName && e.Email == deletemployee.Email && e.UserName == deletemployee.UserName && e.EmployeeNumber == deletemployee.EmployeeNumber).FirstOrDefault();
                    db.Employees.DeleteOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                case "create":
                    var newEmployee = employee;
                    newEmployee = db.Employees.Where(e => e.FirstName == newEmployee.FirstName && e.LastName == newEmployee.LastName && e.Email == newEmployee.Email && e.UserName == newEmployee.UserName && e.EmployeeNumber == newEmployee.EmployeeNumber).FirstOrDefault();
                    db.Employees.InsertOnSubmit(newEmployee);
                    db.SubmitChanges();
                    break;
                default:
                    Console.WriteLine("Wrong input please try again");
                    break;
            }

            Console.ReadLine();
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
            var animal = db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
            return animal;

        }
        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            Animal animal = new Animal();
            var updateanimal = animal;
            updateanimal = db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault();
            foreach (KeyValuePair<int, string> entry in updates)
            {
    
                switch (entry.Key)
                {
                    case 1:
                       updateanimal = db.Animals.Where(a => a.Category == updateanimal.Category).FirstOrDefault();
                        break;
                    case 2:
                        updateanimal = db.Animals.Where(a => a.Name == updateanimal.Name).FirstOrDefault();
                        break;
                    case 3:
                        updateanimal = db.Animals.Where(a => a.Age == updateanimal.Age).FirstOrDefault();
                        break;
                    case 4:
                        updateanimal = db.Animals.Where(a => a.Demeanor == updateanimal.Demeanor).FirstOrDefault();
                        break;
                    case 5:
                        updateanimal = db.Animals.Where(a => a.KidFriendly == updateanimal.KidFriendly).FirstOrDefault();
                        break;
                    case 6:
                        updateanimal = db.Animals.Where(a => a.PetFriendly == updateanimal.PetFriendly).FirstOrDefault();
                        break;
                    case 7:
                        updateanimal = db.Animals.Where(a => a.Weight == updateanimal.Weight).FirstOrDefault();
                        break;
                    case 8:
                        Console.WriteLine("Finished");
                        return;
                    default:
                        Console.WriteLine("please input your choice");
                        break;
                }
                Console.ReadLine();

            }
        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }

        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {


            throw new NotImplementedException();


        }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            var categoryondb = db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
            return categoryondb.CategoryId;
        }

        internal static Room GetRoom(int animalId)
        {
            Room roomfromdb = db.Rooms.Where(r => r.AnimalId == animalId).FirstOrDefault();
            return roomfromdb;
        }

        internal static int GetDietPlanId(string dietPlanName)
        {
            var dietplanondb = db.DietPlans.Where(d => d.Name == dietPlanName).FirstOrDefault();
            return dietplanondb.DietPlanId;
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
          var thisAdoption = db.Adoptions.Where(a => a.ApprovalStatus == "false" && a.ClientId == client.ClientId && a.AnimalId == animal.AnimalId && a.AdoptionFee == 75 && a.PaymentCollected == false).FirstOrDefault();
         db.Adoptions.InsertOnSubmit(thisAdoption);
         db.SubmitChanges();
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            var getAdoption = db.Adoptions.Where(a => a.ApprovalStatus == "pending");
            return getAdoption;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        { 
           var thisadoption = adoption;
           if (isAdopted == true)
           {
                thisadoption = db.Adoptions.Where(a => a.ApprovalStatus == "true").FirstOrDefault();
                db.Adoptions.InsertOnSubmit(thisadoption);
                db.SubmitChanges();
           }
           else if(isAdopted == false)
           {
                thisadoption = db.Adoptions.Where(a => a.ApprovalStatus == "false").FirstOrDefault();
                db.Adoptions.InsertOnSubmit(thisadoption);
                db.SubmitChanges();
           }
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            var removeThisAdoption = db.Adoptions.Where(a => a.AnimalId == animalId && a.ClientId == clientId).FirstOrDefault();
            db.Adoptions.DeleteOnSubmit(removeThisAdoption);
            db.SubmitChanges();

        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            var getShotsfromdb = db.AnimalShots.Where(a => a.Animal == animal );
            return getShotsfromdb;


        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            
            var shotfromdb = db.Shots.Where(s => s.Name == shotName).FirstOrDefault(); 

            
             db.Shots.Where(s => s.AnimalShots == null);
            foreach (AnimalShot entry in animal.AnimalShots) 
            {
                switch (animal.AnimalId) 
                {
                   

                        
                
                }
            
            }

            db.SubmitChanges();
          
        }




    }
}