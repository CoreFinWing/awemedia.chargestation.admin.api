﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler
{
    public class ErrorHandler :IErrorHandler
    {
        public string GetMessage(ErrorMessagesEnum message)
        {
            switch (message)
            {
                case ErrorMessagesEnum.EntityNull:
                    return "The entity passed is null {0}. Additional information: {1}";

                case ErrorMessagesEnum.ModelValidation:
                    return "The request data is not correct. Additional information: {0}";

                case ErrorMessagesEnum.AuthUserDoesNotExists:
                    return "The user doesn't not exists";

                case ErrorMessagesEnum.AuthWrongCredentials:
                    return "The email or password are wrong";

                case ErrorMessagesEnum.AuthCannotCreate:
                    return "Cannot create user";

                case ErrorMessagesEnum.AuthCannotDelete:
                    return "Cannot delete user";

                case ErrorMessagesEnum.AuthCannotUpdate:
                    return "Cannot update user";

                case ErrorMessagesEnum.AuthNotValidInformations:
                    return "Invalid informations";

                case ErrorMessagesEnum.AuthCannotRetrieveToken:
                    return "Cannot retrieve token";
                case ErrorMessagesEnum.PostedDataNotFound:
                    return "Request body not found";
                case ErrorMessagesEnum.InternalServerError:
                    return "Internal server error";
                case ErrorMessagesEnum.IncorrectDeviceId:
                    return "Incorrect Device Id";
                case ErrorMessagesEnum.DeviceNotRegistered:
                    return "Device not registered";
                case ErrorMessagesEnum.NoCacheHeaderFound:
                    return "No Cache-control header is found";
                case ErrorMessagesEnum.DuplicateRecordFound:
                    return "Duplicate Record Found";
                case ErrorMessagesEnum.BodyNotFound:
                    return "Request Body not found";
                case ErrorMessagesEnum.BranchIsRequired:
                    return "Merchant shoud have atleast one branch.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }

        }
    }
}
