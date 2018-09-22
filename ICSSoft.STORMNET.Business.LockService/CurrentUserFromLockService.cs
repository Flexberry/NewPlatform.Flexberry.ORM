using System;
using System.Collections.Generic;
using System.Text;
using ICSSoft.STORMNET.Business;

namespace ICSSoft.Services
{
    public class CurrentUserFromLockService : CurrentUserService.IUser
    {
        #region IUser Members
        public string FriendlyName
        {
            get
            {
                return LockService.GetUserName();
            }

            set
            {
                throw new NotSupportedException();

                // LockService.SetUserName(value);
            }
        }

        public string Domain
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public string Login
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}
