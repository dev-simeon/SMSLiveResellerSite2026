using Blazored.LocalStorage;

namespace SMSLiveResellerSite2026.Services
{
    public class StorageService(ILocalStorageService storage)
    {
        //private const string storageKey = "UserSession";

        //public ValueTask SetUserSession(LoginResponse response)
        //{
        //    return storage.SetItemAsync(storageKey, response);
        //}

        //public ValueTask<LoginResponse?> GetUserSession()
        //{
        //    return storage.GetItemAsync<LoginResponse>(storageKey);
        //}

        //public ValueTask ClearUserSession()
        //{
        //    return storage.RemoveItemAsync(storageKey);
        //}

    }
}
