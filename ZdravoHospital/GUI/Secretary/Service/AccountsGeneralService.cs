using Model;
using Repository.CredentialsPersistance;
using System.Collections.Generic;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class AccountsGeneralService
    {
        private ICredentialsRepository _credentialsRepository;

        public AccountsGeneralService(ICredentialsRepository credentialsRepository)
        {
            _credentialsRepository = credentialsRepository;
        }
        public List<Credentials> GetAccounts()
        {
            return _credentialsRepository.GetValues();
        }
        public RoleType FindRoleByUsername(string username)
        {
            List<Credentials> accounts = GetAccounts();
            foreach (var account in accounts)
            {
                if (account.Username.Equals(username))
                    return account.Role;
            }
            return RoleType.PATIENT;
        }
        public int GetRoleCount(RoleType role)
        {
            List<Credentials> accounts = GetAccounts();
            int count = 0;
            foreach (var account in accounts)
            {
                if (account.Role.Equals(role))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
