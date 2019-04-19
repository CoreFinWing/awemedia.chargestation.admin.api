using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler
{
    public interface IErrorHandler
    {
        string GetMessage(ErrorMessagesEnum message);
    }
    public enum ErrorMessagesEnum
    {
        EntityNull = 1,
        ModelValidation = 2,
        AuthUserDoesNotExists = 3,
        AuthWrongCredentials = 4,
        AuthCannotCreate = 5,
        AuthCannotDelete = 6,
        AuthCannotUpdate = 7,
        AuthNotValidInformations = 8,
        AuthCannotRetrieveToken = 9,
        PostedDataNotFound=10,
        InternalServerError=11,
        IncorrectDeviceId=12,
        DeviceNotRegistered=13,
        NoCacheHeaderFound=14
    }
}
