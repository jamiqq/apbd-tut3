using System;

namespace LegacyApp
{
    public class UserService
    {      
        private readonly IUserRepository _userRepository;
        
        public UserService() : this(new UserRepository()) { }
        public UserService(IUserRepository userRepository)
        {
            _userRepository = new UserRepository();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUser(firstName, lastName, email, dateOfBirth))
            {   
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);
            if (!SetCreditLimit(user, client))
            {
                return false;
            }
            _userRepository.AddUser(user);
            return true;
        }

        private bool ValidateUser(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                return false;
            }
            var age = CalculateAge(dateOfBirth);
            if (age < 21)
            {
                return false;
            }

            return true;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                DateOfBirth = dateOfBirth,
                Client = client
            };
        }

        private bool SetCreditLimit(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else
            {
                var credit = new UserCreditService();
                var creditLimit = credit.GetCreditLimit(user.LastName, user.DateOfBirth);
                if (client.Type == "ImportantClient")
                {
                    creditLimit *= 2;
                }
                user.CreditLimit = creditLimit;
                user.HasCreditLimit = true;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            return true;
        }
    }
}
